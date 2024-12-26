using Logistics.Controllers;
using Logistics.Data;
using Logistics.Data.Account.Models;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Requests.DTOs.Requests;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Transportations.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Logistics.Data.Documents.Models;
using Logistics.Data.Common;
using System.Linq.Expressions;

namespace Logistics.Services
{
    public class RequestService
    {
        AppDBContext _context;
        public RequestService(AppDBContext context)
        {
            _context = context;
        }

        private Shipper getShipperById(Guid shipperId)
        {
            Shipper? shipper = _context.Shippers.Where(x => x.id == shipperId).FirstOrDefault();
            if (shipper == null)
            {
                throw new ErrorException(404, "Грузоотправитель с таким id не найден");
            }
            return shipper;
        }

        private Transporter getTransporterWithTruckById(Guid transporterId)
        {
            Transporter? transporter = _context.Transporters.Where(x => x.id == transporterId).Include(x => x.truck).FirstOrDefault();
            if (transporter == null)
            {
                throw new ErrorException(404, "Перевозчик с таким id не найден");
            }
            return transporter;
        }

        private Request getRequestById(Guid requestId)
        {
            Request? request = _context.Requests.Include(x => x.shipment).Where(x => x.id == requestId).FirstOrDefault();
            if (request == null)
            {
                throw new ErrorException(404, "Заявка не найдена");
            }
            return request;
        }

        private Request getRequestByIdAndShipperId(Guid requestId, Guid shipperId)
        {
            Request? request = _context.Requests.Include(x => x.shipment).Where(x => x.id == requestId).FirstOrDefault();
            Guid requestShipperId = _context.Requests.Where(x => x.id == requestId).Select(x => x.shipper.id).FirstOrDefault();

            if (request == null)
            {
                throw new ErrorException(404, "Заявка не найдена");
            }
            else if (requestShipperId != shipperId)
            {
                throw new ErrorException(403, "Заявка не принадлежит вам");
            }
            return request;
        }

        private void CheckIfCanInteractWithRequest(User user, out List<string> error)
        {
            Passport? passport = _context.Passports.Where(x => x.user.id == user.id).FirstOrDefault();
            error = new List<string>() { "Необходимо заполнить и указать: " };

            if (!user.haveFilledInCompany()) error.Add("раздел 'О компании', ");
            if (passport == null) error.Add("паспорт в разделе 'Документы', ");
            else if (!passport.haveScan()) error.Add("скан паспорта, ");
        }

        private void CheckIfCanInteractWithRequest(Shipper shipper)
        {
            List<string> error;

            CheckIfCanInteractWithRequest(shipper, out error);

            if (error.Count > 1)
            {
                int lastCommaIndex = error.LastIndexOf(",");
                string s = String.Join("", error);
                throw new ErrorException(400, s.Substring(0, s.Length - 2) + ".");
            }
        }

        private void CheckIfCanInteractWithRequest(Transporter transporter)
        {
            List<string> error;

            CheckIfCanInteractWithRequest(transporter, out error);

            DriverLicense? driverLicense = _context.Licenses.Where(x => x.transporter.id == transporter.id).FirstOrDefault();

            if (driverLicense == null) error.Add("водительское удостоверение в разделе 'Документы', ");
            else if (!driverLicense.haveScan()) error.Add("скан водительского удостоверения, ");

            if (error.Count > 1)
            {
                int lastCommaIndex = error.LastIndexOf(",");
                string s = String.Join("", error);
                throw new ErrorException(400, s.Substring(0, s.Length - 2) + ".");
            }
        }


        private static bool TransporterSuitableForRequest(Request request, Transporter transporter)
        {
            if (request.status != RequestStatus.Active) return false;
            if (request.loadCity != transporter.permanentResidence) return false;
            if (request.shipment.weightInTons > transporter.truck.loadCapacityInTons) return false;
            if (request.truckType != transporter.truck.truckType) return false;
            return true;
        }

        private static Expression<Func<Request, bool>> TransporterSuitableForRequestExpression(Transporter transporter)
        {
            return request =>
                request.status == RequestStatus.Active &&
                request.loadCity == transporter.permanentResidence &&
                request.shipment.weightInTons <= transporter.truck.loadCapacityInTons &&
                request.truckType == transporter.truck.truckType;
        }


        public async Task<ActionResult> CreateRequest(Guid shipperId, RequestDTO createRequest, bool isDelayed)
        {
            Shipper shipper = getShipperById(shipperId);

            createRequest.validate();

            CheckIfCanInteractWithRequest(shipper);

            Request newRequest = new Request(createRequest, shipper, isDelayed);

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetShipperRequests(Guid shipperId, RequestStatus[] statuses)
        {
            if (statuses.Contains(RequestStatus.Rejected)) return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Для грузоотправителя нет типа заявки 'Отклоненная'"));

            List<Request> requests = _context.Requests.Where(x => x.shipper.id == shipperId && statuses.Contains(x.status)).Include(x => x.shipment).ToList();

            var response = requests.Select(x => new ShipperRequestResponse(x)).ToList();
 
            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetTransporterRequests(Guid transporterId, RequestStatus status)
        {
            if (status == RequestStatus.Delayed || status == RequestStatus.ArchivedNotAccepted) return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Для перевозчика нет такого типа заявки"));

            Transporter transporter = getTransporterWithTruckById(transporterId);

            var query = _context.Requests.Include(x => x.shipment).Include(x => x.shipper).AsQueryable();

            List<Request> requests;
            if (status != RequestStatus.Active && status != RequestStatus.Rejected)
            {
                requests = query.Where(x => x.transportation.transporter.id == transporterId && x.status == status).ToList();
            }
            else
            {
                CheckIfCanInteractWithRequest(transporter);

                query = query.Where(TransporterSuitableForRequestExpression(transporter));
                List<Guid> rejectedRequestIds = _context.RejectedRequests.Where(x => x.transporterId == transporterId).Select(x => x.requestId).ToList();

                if (status == RequestStatus.Active)
                {
                    requests = query.Where(x => !rejectedRequestIds.Contains(x.id)).ToList();
                }
                else
                {
                    requests = query.Where(x => rejectedRequestIds.Contains(x.id)).ToList();
                }
            }

            var response = requests.Select(x => new TransporterRequestResponse(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> EditRequest(Guid requestId, Guid shipperId, RequestDTO editRequest)
        {
            Request request = getRequestByIdAndShipperId(requestId, shipperId);

            if (request.status != RequestStatus.Active && request.status != RequestStatus.Delayed) return new ObjectResult(new ErrorResponse(403, "Редактировать можно только активные и отложенные заявки")) { StatusCode = StatusCodes.Status403Forbidden };

            editRequest.validate();

            request.edit(editRequest);

            _context.Requests.Update(request);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> DeleteRequest(Guid requestId, Guid shipperId)
        {
            Request request = getRequestByIdAndShipperId(requestId, shipperId);

            if (request.status != RequestStatus.Active && request.status != RequestStatus.Delayed) return new ObjectResult(new ErrorResponse(403, "Удалять можно только активные и отложенные заявки")) { StatusCode = StatusCodes.Status403Forbidden };

            _context.Requests.Remove(request);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> ChangeRequestCost(Guid requestId, Guid shipperId, ChangeCost change, float? amount)
        {
            Request request = getRequestByIdAndShipperId(requestId, shipperId);

            if (request.status != RequestStatus.Active && request.status != RequestStatus.Delayed) return new ObjectResult(new ErrorResponse(403, "Изменять стоимость можно только у активных и отложенных заявок")) { StatusCode = StatusCodes.Status403Forbidden };

            switch (change)
            {
                case ChangeCost.Increase:
                    {
                        if (amount.HasValue)
                        {
                            request.additionalCostInRubles += amount.Value;
                        }
                        else return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Предоставьте значение на которое будет увеличена цена"));
                        break;
                    }
                case ChangeCost.Reduce:
                    {
                        if (amount.HasValue)
                        {
                            if (request.additionalCostInRubles - amount.Value < 0) return new ConflictObjectResult(new ErrorResponse(409, "Цена не может быть меньше изначальной"));
                            request.additionalCostInRubles -= amount.Value;
                        }
                        else return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Предоставьте значение на которое будет снижена цена"));
                        break;
                    }
                case ChangeCost.Initial:
                    {
                        request.additionalCostInRubles = 0;
                        break;
                    }
            }

            _context.Requests.Update(request);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> AcceptRequest(Guid requestId, Guid transporterId)
        {
            Request request = getRequestById(requestId);
            Transporter transporter = getTransporterWithTruckById(transporterId);

            if (!TransporterSuitableForRequest(request, transporter)) return new ForbidResult();

            CheckIfCanInteractWithRequest(transporter);

            _context.RejectedRequests.Where(x => x.requestId == requestId).ExecuteDelete();

            Transportation transportation = new Transportation(transporter);
            TransportationStatusChange transportationStatusChange = new TransportationStatusChange(transportation, TransportationStatus.WaitingForStart);

            request.status = RequestStatus.Accepted;
            request.transportation = transportation;

            _context.TransportationStatusChanges.Add(transportationStatusChange);
            _context.Requests.Update(request);
            _context.Transportations.Add(transportation);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> RejectRequest(Guid requestId, Guid transporterId)
        {
            Request request = getRequestById(requestId);
            Transporter transporter = getTransporterWithTruckById(transporterId);
            
            if (!TransporterSuitableForRequest(request, transporter)) return new ForbidResult();

            CheckIfCanInteractWithRequest(transporter);

            RejectedRequest? rejectedRequest = _context.RejectedRequests.Where(x => x.requestId == requestId && x.transporterId == transporterId).FirstOrDefault();
            if (rejectedRequest != null)
            {
                return new ObjectResult(new ErrorResponse(403, "Вы уже отклонили эту заявку")) { StatusCode = StatusCodes.Status403Forbidden };
            }

            rejectedRequest = new RejectedRequest(transporterId, requestId);

            _context.RejectedRequests.Add(rejectedRequest);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> PublishDelayedRequest(Guid requestId, Guid shipperId)
        {
            Request request = getRequestByIdAndShipperId(requestId, shipperId);
            Shipper shipper = getShipperById(shipperId);

            CheckIfCanInteractWithRequest(shipper);

            if (request.status != RequestStatus.Delayed) return new NotFoundObjectResult(new ErrorResponse(404, "Нет отложенной заявки с таким id"));

            Passport passport = _context.Passports.Where(x => x.user.id == shipperId).FirstOrDefault();
            if (!(request.shipper.haveFilledInCompany() && passport != null && passport.haveScan())) return new ObjectResult(new ErrorResponse(403, "Необходимо заполнить разделы 'О компании' и 'Документы' в профиле")) { StatusCode = StatusCodes.Status403Forbidden };

            request.status = RequestStatus.Active;

            _context.Requests.Update(request);
            _context.SaveChanges();

            return new OkObjectResult("");
        }
    }
}

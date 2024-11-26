using Geocoding.Google;
using Geocoding;
using Logistics.Controllers;
using Logistics.Data;
using Logistics.Data.Account.Models;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Requests.RequestDTOs.Requests;
using Logistics.Data.Requests.RequestDTOs.Responses;
using Logistics.Data.Transportations.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Logistics.Services
{
    public class RequestService
    {
        AppDBContext _context;
        public RequestService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> CreateRequest(Guid shipperId, CreateRequestRequestDTO createRequest, bool isDelayed)
        {
            Shipper shipper = _context.Shippers.Where(x => x.id == shipperId).FirstOrDefault();
            if (shipper == null) {
                return new UnauthorizedObjectResult("");
            }

            Request newRequest = new Request(createRequest, shipper, isDelayed);

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetShipperRequests(Guid shipperId, RequestStatus status)
        {
            List<Request> requests = _context.Requests.Where(x => x.shipper.id == shipperId && x.status == status).ToList();

            var response = requests.Select(x => new RequestResponse(x)).ToList();
 
            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetTransporterRequests(Guid transporterId, RequestStatus status)
        {
            List<Request> requests;

            Transporter transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();

            if (status == RequestStatus.Accepted) requests = _context.Requests.Where(x => x.status == RequestStatus.Active && x.loadCity == transporter.permanentResidence).ToList();
            else requests = _context.Requests.Where(x => x.transportation.transporter.id == transporterId && x.status == status).ToList();

            var response = requests.Select(x => new RequestResponse(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> EditRequest(Guid requestId, Guid shipperId, EditRequestRequestDTO editRequest)
        {
            Request request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            else if (request.shipper.id != shipperId)
            {
                return new ForbidResult();
            }

            request.edit(editRequest);

            _context.Requests.Update(request);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> DeleteRequest(Guid requestId, Guid shipperId)
        {
            Request request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            else if (request.shipper.id != shipperId)
            {
                return new ForbidResult();
            }

            _context.Requests.Remove(request);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> ChangeRequestCost(Guid requestId, Guid shipperId, ChangeCost change, float? amount)
        {
            Request request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();

            if (request == null) {
                return new NotFoundObjectResult(null);
            }
            else if (request.shipper.id != shipperId) {
                return new ForbidResult();
            }

            switch (change)
            {
                case ChangeCost.Increase:
                    {
                        if (amount.HasValue)
                        {
                            request.costInRubles += amount.Value;
                        }
                        else return new BadRequestObjectResult(new ErrorResponse(400, "Предоставьте значение на которое будет увеличена цена"));
                        break;
                    }
                case ChangeCost.Reduce:
                    {
                        if (amount.HasValue)
                        {
                            if (request.costInRubles - amount.Value < 0) return new ConflictObjectResult(new ErrorResponse(409, "Цена не может быть меньше изначальной"));
                            request.costInRubles -= amount.Value;
                        }
                        else return new BadRequestObjectResult(new ErrorResponse(400, "Предоставьте значение на которое будет снижена цена"));
                        break;
                    }
                case ChangeCost.Initial:
                    {
                        request.costInRubles = 0;
                        break;
                    }
            }

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> AcceptRequest(Guid requestId, Guid transporterId)
        {
            Request? request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();
            if (request == null) return new NotFoundObjectResult(new ErrorResponse(404, "Заявки с таким id нет"));

            Transporter transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault()!;

            if (request.loadCity != transporter.permanentResidence) return new ForbidResult();

            Transportation transportation = new Transportation(request, transporter);

            request.status = RequestStatus.Accepted;

            _context.Requests.Update(request);
            _context.Transportations.Add(transportation);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }
    }
}

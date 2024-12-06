﻿using Logistics.Controllers;
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
using Microsoft.AspNetCore.Http.HttpResults;

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
            Passport passport = _context.Passports.Where(x => x.user.id == shipperId).FirstOrDefault();

            if (!(shipper.haveFilledInCompany() && passport != null && passport.haveScan())) return new ObjectResult(new ErrorResponse(403, "Необходимо заполнить разделы 'О компании' и 'Документы' в профиле")) { StatusCode = StatusCodes.Status403Forbidden };

            Request newRequest = new Request(createRequest, shipper, isDelayed);

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetShipperRequests(Guid shipperId, RequestStatus[] statuses)
        {
            List<Request> requests = _context.Requests.Where(x => x.shipper.id == shipperId && statuses.Contains(x.status)).Include(x => x.shipment).ToList();

            var response = requests.Select(x => new ShipperRequestResponse(x)).ToList();
 
            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetTransporterRequests(Guid transporterId, RequestStatus status)
        {
            List<Request> requests;

            Transporter transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();

            if (status == RequestStatus.Active) requests = _context.Requests.Where(x => x.status == RequestStatus.Active && x.loadCity == transporter.permanentResidence).Include(x => x.shipment).Include(x => x.shipper).ToList();
            else requests = _context.Requests.Include(x => x.transportation).Where(x => x.transportation.transporter.id == transporterId && x.status == status).Include(x => x.shipment).Include(x => x.shipper).ToList();

            var response = requests.Select(x => new TransporterRequestResponse(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> EditRequest(Guid requestId, Guid shipperId, EditRequestRequestDTO editRequest)
        {
            Request request = _context.Requests.Include(x => x.shipment).Where(x => x.id == requestId).FirstOrDefault();
            Guid requestShipperId = _context.Requests.Where(x => x.id == requestId).Select(x => x.shipper.id).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            else if (requestShipperId != shipperId)
            {
                return new ForbidResult();
            }
            if (request.status != RequestStatus.Active && request.status != RequestStatus.Delayed) return new ObjectResult(new ErrorResponse(403, "Редактировать можно только активные и отложенные заявки")) { StatusCode = StatusCodes.Status403Forbidden };

            request.edit(editRequest);

            _context.Requests.Update(request);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> DeleteRequest(Guid requestId, Guid shipperId)
        {
            Request request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();
            Guid requestShipperId = _context.Requests.Where(x => x.id == requestId).Select(x => x.shipper.id).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            else if (requestShipperId != shipperId)
            {
                return new ForbidResult();
            }
            if (request.status != RequestStatus.Active && request.status != RequestStatus.Delayed) return new ObjectResult(new ErrorResponse(403, "Удалять можно только активные и отложенные заявки")) { StatusCode = StatusCodes.Status403Forbidden };

            _context.Requests.Remove(request);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> ChangeRequestCost(Guid requestId, Guid shipperId, ChangeCost change, float? amount)
        {
            Request request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();
            Guid requestShipperId = _context.Requests.Where(x => x.id == requestId).Select(x => x.shipper.id).FirstOrDefault();

            if (request == null) {
                return new NotFoundObjectResult(null);
            }
            else if (requestShipperId != shipperId) {
                return new ForbidResult();
            }
            if (request.status != RequestStatus.Active && request.status != RequestStatus.Delayed) return new ObjectResult(new ErrorResponse(403, "Изменять стоимость можно только у активных и отложенных заявок")) { StatusCode = StatusCodes.Status403Forbidden };

            switch (change)
            {
                case ChangeCost.Increase:
                    {
                        if (amount.HasValue)
                        {
                            request.additionalCostInRubles += amount.Value;
                        }
                        else return new BadRequestObjectResult(new ErrorResponse(400, "Предоставьте значение на которое будет увеличена цена"));
                        break;
                    }
                case ChangeCost.Reduce:
                    {
                        if (amount.HasValue)
                        {
                            if (request.additionalCostInRubles - amount.Value < 0) return new ConflictObjectResult(new ErrorResponse(409, "Цена не может быть меньше изначальной"));
                            request.additionalCostInRubles -= amount.Value;
                        }
                        else return new BadRequestObjectResult(new ErrorResponse(400, "Предоставьте значение на которое будет снижена цена"));
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

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> AcceptRequest(Guid requestId, Guid transporterId)
        {
            Request? request = _context.Requests.Where(x => x.id == requestId).FirstOrDefault();
            if (request == null) return new NotFoundObjectResult(new ErrorResponse(404, "Заявки с таким id нет"));
            if (request.status != RequestStatus.Active) return new ForbidResult();

            Transporter transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault()!;

            Request? acceptedRequest = _context.Requests.Where(x => x.transportation.transporter.id == transporterId && x.status == RequestStatus.Accepted).FirstOrDefault();
            if (acceptedRequest != null) return new ObjectResult(new ErrorResponse(403, "Вы не можете одновременно принять больше одной заявки")) { StatusCode = StatusCodes.Status403Forbidden };
            if (request.loadCity != transporter.permanentResidence) return new ForbidResult();

            Transportation transportation = new Transportation(transporter);

            TransportationStatusChange transportationStatusChange = new TransportationStatusChange(transportation, TransportationStatus.WaitingForStart);

            request.status = RequestStatus.Accepted;
            request.transportation = transportation;

            _context.TransportationStatusChanges.Add(transportationStatusChange);
            _context.Requests.Update(request);
            _context.Transportations.Add(transportation);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> PublishDelayedRequest(Guid requestId, Guid shipperId)
        {
            Request request = _context.Requests.Where(x => x.id == requestId).Include(x => x.shipper).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            else if (request.shipper.id != shipperId)
            {
                return new ForbidResult();
            }
            if (request.status != RequestStatus.Delayed) return new NotFoundObjectResult(new ErrorResponse(404, "Нет отложенной заявки с таким id"));

            Passport passport = _context.Passports.Where(x => x.user.id == shipperId).FirstOrDefault();
            if (!(request.shipper.haveFilledInCompany() && passport != null && passport.haveScan())) return new ObjectResult(new ErrorResponse(403, "Необходимо заполнить разделы 'О компании' и 'Документы' в профиле")) { StatusCode = StatusCodes.Status403Forbidden };

            request.status = RequestStatus.Active;

            _context.Requests.Update(request);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }
    }
}

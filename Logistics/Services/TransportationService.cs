﻿using Logistics.Data;
using Logistics.Data.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logistics.Data.Transportations.DTOs.Responses;
using Logistics.Data.Transportations.Models;
using Microsoft.EntityFrameworkCore;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Account.Models;
using System.Globalization;
using Logistics.Data.Common;

namespace Logistics.Services
{
    public class TransportationService
    {
        private AppDBContext _context;

        private Dictionary<TransportationStatus, List<TransportationStatus>> possibleTransportationStatusChange;
        public TransportationService(AppDBContext context)
        {
            _context = context;
            possibleTransportationStatusChange = new Dictionary<TransportationStatus, List<TransportationStatus>>
            {
                { TransportationStatus.WaitingForStart, new List<TransportationStatus>{ TransportationStatus.OnWayToLoading } },
                { TransportationStatus.OnWayToLoading, new List<TransportationStatus>{ TransportationStatus.Loading, TransportationStatus.Repairing } },
                { TransportationStatus.Loading, new List<TransportationStatus>{ TransportationStatus.OnWayToUnloading } },
                { TransportationStatus.OnWayToUnloading, new List<TransportationStatus>{ TransportationStatus.Unloading, TransportationStatus.Repairing } },
                { TransportationStatus.Repairing, new List<TransportationStatus>{ TransportationStatus.OnWayToLoading, TransportationStatus.OnWayToUnloading } },
                { TransportationStatus.Unloading, new List<TransportationStatus>{ TransportationStatus.Finished } },
                { TransportationStatus.Finished, new List<TransportationStatus>{  } },
            };
        }

        private Transportation getTransportationByIdAndTransporterId(Guid transportationId, Guid transporterId)
        {
            Transportation? transportation = _context.Transportations.Where(x => x.id == transportationId).Include(x => x.request).Include(x => x.request.shipper).Include(x => x.request.shipment).FirstOrDefault();
            Guid transportationTransporterId = _context.Transportations.Where(x => x.id == transportationId).Select(x => x.transporter.id).FirstOrDefault();

            if (transportation == null)
            {
                throw new ErrorException(404, "Нет перевозки с таким id");
            }
            if (transportationTransporterId != transporterId)
            {
                throw new ErrorException(403, "У вас нет доступа к этой перевозке");
            }

            return transportation;
        }

        public async Task<ActionResult> GetTransporterTransportations(Guid transporterId, bool isFinished)
        {
            List<Transportation> transportations = _context.Transportations.Where(x => (isFinished ? x.status == TransportationStatus.Finished : x.status != TransportationStatus.Finished) && x.transporter.id == transporterId).Include(x => x.request).Include(x => x.request.shipper).Include(x => x.request.shipment).ToList();

            var response = transportations.Select(x => new TransporterTransportationResponseDTO(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetTransporterTransportation(Guid transporterId, Guid transportationId)
        {
            Transportation transportation = getTransportationByIdAndTransporterId(transportationId, transporterId);

            List<TransportationStatusChangeResponseDTO> statusChangeHistory = _context.TransportationStatusChanges.Where(x => x.transportation.id == transportationId).OrderBy(x => x.time).Select(x => new TransportationStatusChangeResponseDTO(x)).ToList();

            return new OkObjectResult(new TransporterTransportationWideResponseDTO(transportation, statusChangeHistory));
        }

        public async Task<ActionResult> SetTransportationStatus(Guid transporterId, Guid transportationId, TransportationStatus status)
        {
            Transportation transportation = getTransportationByIdAndTransporterId(transportationId, transporterId);

            if (!possibleTransportationStatusChange[transportation.status].Contains(status)) return new ObjectResult(new ErrorResponse(403, "Невозможно присвоить данный статус в данный момент")) { StatusCode = StatusCodes.Status403Forbidden };

            if (status == TransportationStatus.Finished)
            {
                _context.Requests.Where(x => x.transportation == transportation).ExecuteUpdate(x => x.SetProperty(x => x.status, RequestStatus.ArchivedTransportationFinished));
            }
            else if (status == TransportationStatus.Unloading)
            {
                _context.Requests.Where(x => x.transportation == transportation).ExecuteUpdate(x => x.SetProperty(x => x.arrivalTime, DateTime.UtcNow));
            }

            transportation.status = status;
            TransportationStatusChange newChange = new TransportationStatusChange(transportation, status);

            _context.Transportations.Update(transportation);
            _context.TransportationStatusChanges.Add(newChange);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetShipperTransportations(Guid shipperId, bool isFinished)
        {
            List<Transportation> transportations = _context.Transportations.Where(x => (isFinished ? x.status == TransportationStatus.Finished : x.status != TransportationStatus.Finished) && x.request.shipper.id == shipperId).Include(x => x.transporter).Include(x => x.transporter.truck).Include(x => x.request).ToList();

            var response = transportations.Select(x => new ShipperTransportationResponseDTO(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetShipperTransportation(Guid shipperId, Guid transportationId)
        {
            Transportation? transportation = _context.Transportations.Where(x => x.id == transportationId).Include(x => x.request).Include(x => x.transporter).Include(x => x.transporter.truck).Include(x => x.request.shipment).FirstOrDefault();
            Guid requestShipperId = _context.Transportations.Where(x => x.id == transportationId).Select(x => x.request.shipper.id).FirstOrDefault();

            if (transportation == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Нет перевозки с таким id"));
            }
            if (requestShipperId != shipperId)
            {
                return new ObjectResult(new ErrorResponse(403, "У вас нет доступа к этой перевозке")) { StatusCode = StatusCodes.Status403Forbidden };
            }

            List<TransportationStatusChangeResponseDTO> statusChangeHistory = _context.TransportationStatusChanges.Where(x => x.transportation.id == transportationId).OrderBy(x => x.time).Select(x => new TransportationStatusChangeResponseDTO(x)).ToList();

            return new OkObjectResult(new ShipperTransportationWideResponseDTO(transportation, statusChangeHistory));
        }

        public async Task<ActionResult> CreateReview(Guid reviewerId, Guid transportationId, string reviewText)
        {
            Transportation? transportation = _context.Transportations.Where(x => x.id == transportationId).FirstOrDefault();
            if (transportation == null) { return new NotFoundObjectResult(new ErrorResponse(404, "Перевозки с таким id не существует")); }

            var userIds = new List<Guid> { transportation.transporter.id, transportation.request.shipper.id };
            if (!userIds.Contains(reviewerId) || transportation.status != TransportationStatus.Finished)
            {
                return new ObjectResult(new ErrorResponse(403, "Вы не можете оставить отзыв по этой перевозке")) { StatusCode = StatusCodes.Status403Forbidden };
            }
            userIds.Remove(reviewerId);

            Review? review = _context.Reviews.Where(x => x.transportationId == transportationId && x.reviewerId == reviewerId).FirstOrDefault();
            if (review != null)
            {
                return new ObjectResult(new ErrorResponse(403, "По этой перевозке вы уже оставили отзыв")) { StatusCode = StatusCodes.Status403Forbidden };
            }

            review = new Review(transportationId, reviewerId, userIds.First(), reviewText);

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return new OkObjectResult("");
        }
    }
}

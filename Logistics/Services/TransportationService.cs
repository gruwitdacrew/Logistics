using Logistics.Data;
using Logistics.Data.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logistics.Data.Transportations.DTOs.Responses;
using Logistics.Data.Account.Models;
using Logistics.Data.Transportations.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Services
{
    public class TransportationService
    {
        AppDBContext _context;
        public TransportationService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetTransporterTransportations(Guid transporterId)
        {
            List<Request> requests = _context.Requests.Where(x => x.transportation.transporter.id == transporterId).Include(x => x.transportation).Include(x => x.shipper).Include(x => x.shipment).ToList();

            var response = requests.Select(x => new TransporterTransportationResponseDTO(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetTransporterTransportation(Guid transporterId, Guid transportationId)
        {
            Request request = _context.Requests.Where(x => x.transportation.id == transportationId).Include(x => x.transportation).Include(x => x.shipper).Include(x => x.shipment).FirstOrDefault();
            Guid transportationTransporterId = _context.Transportations.Where(x => x.id == transportationId).Select(x => x.transporter.id).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            if (transportationTransporterId != transporterId)
            {
                return new ForbidResult();
            }

            List<TransportationStatusChangeResponseDTO> statusChangeHistory = _context.TransportationStatusChanges.Where(x => x.transportation.id == transportationId).Select(x => new TransportationStatusChangeResponseDTO(x)).ToList();

            return new OkObjectResult(new TransporterTransportationWideResponseDTO(request, statusChangeHistory));
        }

        public async Task<ActionResult> SetTransportationStatus(Guid transporterId, Guid transportationId, TransportationStatus status)
        {
            Transportation transportation = _context.Transportations.Where(x => x.id == transportationId).FirstOrDefault();
            Guid transportationTransporterId = _context.Transportations.Where(x => x.id == transportationId).Select(x => x.transporter.id).FirstOrDefault();

            if (transportation == null)
            {
                return new NotFoundObjectResult(null);
            }
            if (transportationTransporterId != transporterId)
            {
                return new ForbidResult();
            }

            transportation.status = status;
            TransportationStatusChange newChange = new TransportationStatusChange(transportation, status);

            _context.Transportations.Update(transportation);
            _context.TransportationStatusChanges.Add(newChange);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetShipperTransportations(Guid shipperId)
        {
            List<Request> requests = _context.Requests.Where(x => x.shipper.id == shipperId).Include(x => x.transportation).Include(x => x.transportation.transporter).Include(x => x.transportation.transporter.truck).ToList();

            var response = requests.Select(x => new ShipperTransportationResponseDTO(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetShipperTransportation(Guid shipperId, Guid transportationId)
        {
            Request request = _context.Requests.Where(x => x.transportation.id == transportationId).Include(x => x.transportation).Include(x => x.transportation.transporter).Include(x => x.transportation.transporter.truck).FirstOrDefault();
            Guid requestShipperId = _context.Requests.Where(x => x.transportation.id == transportationId).Select(x => x.shipper.id).FirstOrDefault();

            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            if (requestShipperId != shipperId)
            {
                return new ForbidResult();
            }

            List<TransportationStatusChangeResponseDTO> statusChangeHistory = _context.TransportationStatusChanges.Where(x => x.transportation.id == transportationId).Select(x => new TransportationStatusChangeResponseDTO(x)).ToList();

            return new OkObjectResult(new ShipperTransportationWideResponseDTO(request, statusChangeHistory));
        }
    }
}

using Logistics.Data;
using Logistics.Data.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logistics.Data.Transportations.DTOs.Responses;
using Logistics.Data.Account.Models;

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
            List<Request> requests = _context.Requests.Where(x => x.transportation.transporter.id == transporterId).ToList();

            var response = requests.Select(x => new TransporterTransportationResponseDTO(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetTransporterTransportation(Guid transporterId, Guid transportationId)
        {
            Request request = _context.Requests.Where(x => x.transportation.id == transportationId).FirstOrDefault();
            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            if (request.shipper.id != transporterId)
            {
                return new ForbidResult();
            }

            List<TransportationStatusChangeResponseDTO> statusChangeHistory = _context.TransportationStatusChanges.Where(x => x.transportation.id == transportationId).Select(x => new TransportationStatusChangeResponseDTO(x)).ToList();

            return new OkObjectResult(new TransporterTransportationWideResponseDTO(request, statusChangeHistory));
        }

        public async Task<ActionResult> GetShipperTransportations(Guid shipperId)
        {
            List<Request> requests = _context.Requests.Where(x => x.shipper.id == shipperId).ToList();

            var response = requests.Select(x => new ShipperTransportationResponseDTO(x)).ToList();

            return new OkObjectResult(response);
        }

        public async Task<ActionResult> GetShipperTransportation(Guid shipperId, Guid transportationId)
        {
            Request request = _context.Requests.Where(x => x.transportation.id == transportationId).FirstOrDefault();
            if (request == null)
            {
                return new NotFoundObjectResult(null);
            }
            if (request.shipper.id != shipperId)
            {
                return new ForbidResult();
            }

            List<TransportationStatusChangeResponseDTO> statusChangeHistory = _context.TransportationStatusChanges.Where(x => x.transportation.id == transportationId).Select(x => new TransportationStatusChangeResponseDTO(x)).ToList();

            return new OkObjectResult(new ShipperTransportationWideResponseDTO(request, statusChangeHistory));
        }
    }
}

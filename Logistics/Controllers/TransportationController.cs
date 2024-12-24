using Logistics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logistics.Data.Transportations.DTOs.Responses;
using Logistics.Data.Transportations.Models;

namespace Logistics.Controllers
{
    [ApiController]
    [Route("api/transportation/")]
    public class TransportationController : ControllerBase
    {
        TransportationService _transportationService;
        public TransportationController(TransportationService transportationService)
        {
            _transportationService = transportationService;
        }

        [Authorize(Roles = "Shipper")]
        [HttpGet]
        [Route("/api/shipper/transportations")]
        [ProducesResponseType(typeof(List<ShipperTransportationResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> getShipperTransportations()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _transportationService.GetShipperTransportations(new Guid(userId));
        }

        [Authorize(Roles = "Shipper")]
        [HttpGet]
        [Route("/api/shipper/transportation/{transportationId}")]
        [ProducesResponseType(typeof(ShipperTransportationWideResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult> getShipperTransportation([FromRoute] Guid transportationId)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _transportationService.GetShipperTransportation(new Guid(userId), transportationId);
        }

        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("/api/transporter/transportations")]
        [ProducesResponseType(typeof(List<TransporterTransportationResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> getTransporterTransportations()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _transportationService.GetTransporterTransportations(new Guid(userId));
        }

        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("/api/transporter/transportation/{transportationId}")]
        [ProducesResponseType(typeof(TransporterTransportationWideResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult> getTransporterTransportation([FromRoute] Guid transportationId)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _transportationService.GetTransporterTransportation(new Guid(userId), transportationId);
        }

        [Authorize(Roles = "Transporter")]
        [HttpPut]
        [Route("/api/transportation/{transportationId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> setTransportationStatus([FromRoute] Guid transportationId, TransportationStatus status)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _transportationService.SetTransportationStatus(new Guid(userId), transportationId, status);
        }

        [Authorize]
        [HttpPost]
        [Route("/api/transportation/{transportationId}/review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> createReview([FromRoute] Guid transportationId, [FromBody] string reviewText)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _transportationService.CreateReview(new Guid(userId), transportationId, reviewText);
        }
    }
}

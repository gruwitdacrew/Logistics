using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Requests.DTOs.Requests;
using Logistics.Services;
using Logistics.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logistics.Data.Requests.DTOs.Responses;

namespace Logistics.Controllers
{
    [ApiController]
    [Route("api/request/")]
    public class RequestController : ControllerBase
    {
        RequestService _requestService;
        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }


        [Authorize(Roles = "Shipper")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> createRequest(CreateRequestRequestDTO createRequest, bool isDelayed)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.CreateRequest(new Guid(userId), createRequest, isDelayed);
        }


        [Authorize(Roles = "Shipper")]
        [HttpPatch]
        [Route("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> editRequest([FromRoute] Guid requestId, EditRequestRequestDTO editRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.EditRequest(requestId, new Guid(userId), editRequest);
        }


        [Authorize(Roles = "Shipper")]
        [HttpGet]
        [Route("/api/shipper/requests")]
        [ProducesResponseType(typeof(List<ShipperRequestResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShipperRequestResponse>> getShipperRequests([FromQuery] RequestStatus[] statuses)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.GetShipperRequests(new Guid(userId), statuses);
        }


        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("/api/transporter/requests")]
        [ProducesResponseType(typeof(List<TransporterRequestResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<TransporterRequestResponse>> getTransporterRequests(RequestStatus status)
        {
            if (status == RequestStatus.Delayed) return new BadRequestObjectResult(new ErrorResponse(400, "Для перевозчика нет такого типа заявки"));

            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.GetTransporterRequests(new Guid(userId), status);
        }


        [Authorize(Roles = "Shipper")]
        [HttpDelete]
        [Route("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> deleteRequest(Guid requestId)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.DeleteRequest(requestId, new Guid(userId));
        }


        [Authorize(Roles = "Shipper")]
        [HttpGet]
        [Route("calculate/cost")]
        [ProducesResponseType(typeof(float), StatusCodes.Status200OK)]
        public async Task<ActionResult<float>> calculateCost(int distanceBetweenCitiesInKilometers)
        {
            return Ok(CostCalculator.calculateCostInRubles(distanceBetweenCitiesInKilometers));
        }


        [Authorize(Roles = "Shipper")]
        [HttpPatch]
        [Route("{requestId}/cost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> changeCost(Guid requestId, ChangeCost change, float? amount)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.ChangeRequestCost(requestId, new Guid(userId), change, amount);
        }


        [Authorize(Roles = "Transporter")]
        [HttpPost]
        [Route("{requestId}/accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> acceptRequest(Guid requestId)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.AcceptRequest(requestId, new Guid(userId));
        }


        [Authorize(Roles = "Shipper")]
        [HttpPost]
        [Route("delayed/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> publishDelayed(Guid requestId)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _requestService.PublishDelayedRequest(requestId, new Guid(userId));
        }
    }

    public enum ChangeCost
    {
        Increase,
        Reduce,
        Initial
    }
}

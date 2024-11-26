using Logistics.Data.Common.CommonDTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Data
{
    public static class InvalidModelStateResponse
    {
        public static IActionResult MakeValidationResponse(ActionContext context)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };
            // My app calls Chat, so, that's why I called this var as chatProblemDetails
            var chatProblemDetails = new CustomProblemDetails
            {
                status = problemDetails.Status,
                errors = problemDetails.Errors,
            };

            var result = new BadRequestObjectResult(chatProblemDetails);

            result.ContentTypes.Add("application/problem+json");

            return result;
        }
    }
}

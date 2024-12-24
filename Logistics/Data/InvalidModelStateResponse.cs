using Logistics.Data.Common.CommonDTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Data
{
    public static class InvalidModelStateResponse
    {
        public static IActionResult MakeValidationResponse(ActionContext context)
        {
            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };

            var problemDetails = new ErrorProblemDetails((int)validationProblemDetails.Status);

            foreach (var error in validationProblemDetails.Errors)
            {
                problemDetails.addError(error.Key, error.Value.First());
            }

            var result = new BadRequestObjectResult(problemDetails);

            result.ContentTypes.Add("application/problem+json");

            return result;
        }
    }
}

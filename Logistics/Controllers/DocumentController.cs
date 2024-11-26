using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Documents.DTOs.Requests;
using Logistics.Data.Documents.DTOs.Responses;
using Logistics.Data.Documents.Models;
using Logistics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Controllers
{
    [ApiController]
    [Route("api/document/")]
    public class DocumentController : ControllerBase
    {
        DocumentService _documentService;
        public DocumentController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        [Authorize]
        [HttpPost]
        [Route("passport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> passport(CreatePassportDTO createPassport)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.CreatePassport(new Guid(userId), createPassport);
        }

        [Authorize]
        [HttpPatch]
        [Route("passport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> passport(EditPassportDTO editPassport)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.EditPassport(new Guid(userId), editPassport);
        }

        [Authorize]
        [HttpGet]
        [Route("passport")]
        [ProducesResponseType(typeof(PassportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PassportResponse>> passport()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.GetPassport(new Guid(userId));
        }

        [Authorize]
        [HttpDelete]
        [Route("passport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> deletePassport()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.DeletePassport(new Guid(userId));
        }




        [Authorize(Roles = "Transporter")]
        [HttpPost]
        [Route("license")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> license(CreateDriverLicenseRequestDTO createLicense)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.CreateDriverLicense(new Guid(userId), createLicense);
        }

        [Authorize(Roles = "Transporter")]
        [HttpPatch]
        [Route("license")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> license(EditDriverLicenseDTO editLicense)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.EditDriverLicense(new Guid(userId), editLicense);
        }

        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("license")]
        [ProducesResponseType(typeof(DriverLicenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DriverLicenseResponse>> license()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.GetDriverLicense(new Guid(userId));
        }

        [Authorize(Roles = "Transporter")]
        [HttpDelete]
        [Route("license")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> deleteLicense()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.DeleteDriverLicense(new Guid(userId));
        }


        [Authorize]
        [HttpPost]
        [Route("passport/scan")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> uploadPassportScan(IFormFile file)
        {
            var userId = User.Claims.ToList()[0].Value;


            if (file == null || file.Length == 0)
                return new BadRequestObjectResult(new ErrorResponse(400, "Файл не выбран"));

            if (file.ContentType != "application/pdf")
                return new BadRequestObjectResult(new ErrorResponse(400, "Файл должен быть в формате pdf"));

            byte[] pdfFileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                pdfFileData = memoryStream.ToArray();
            }


            return await _documentService.UploadPassportScan(new Guid(userId), pdfFileData);
        }


        [Authorize]
        [HttpGet]
        [Route("passport/scan")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> getPassportScan()
        {
            var userId = User.Claims.ToList()[0].Value;

            var response = await _documentService.GetPassportScan(new Guid(userId));

            if (response is OkObjectResult okResult)
            {
                FileContentResult file = new FileContentResult((byte[])okResult.Value, "application/pdf")
                {
                    FileDownloadName = "passport.pdf"
                };
                return file;
            }
            else return response;
        }

        [Authorize]
        [HttpDelete]
        [Route("passport/scan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> deletePassportScan()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.DeletePassportScan(new Guid(userId));
        }


        [Authorize(Roles = "Transporter")]
        [HttpPost]
        [Route("license/scan")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> uploadLicenseScan(IFormFile file)
        {
            var userId = User.Claims.ToList()[0].Value;


            if (file == null || file.Length == 0)
                return new BadRequestObjectResult(new ErrorResponse(400, "Файл не выбран"));

            if (file.ContentType != "application/pdf")
                return new BadRequestObjectResult(new ErrorResponse(400, "Файл должен быть в формате pdf"));

            byte[] pdfFileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                pdfFileData = memoryStream.ToArray();
            }


            return await _documentService.UploadLicenseScan(new Guid(userId), pdfFileData);
        }


        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("license/scan")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> getLicenseScan()
        {
            var userId = User.Claims.ToList()[0].Value;

            var response = await _documentService.GetLicenseScan(new Guid(userId));

            if (response is OkObjectResult okResult)
            {
                FileContentResult file = new FileContentResult((byte[])okResult.Value, "application/pdf")
                {
                    FileDownloadName = "license.pdf"
                };
                return file;
            }
            else return response;
        }

        [Authorize(Roles = "Transporter")]
        [HttpDelete]
        [Route("license/scan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> deleteLicenseScan()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _documentService.DeleteLicenseScan(new Guid(userId));
        }
    }
}

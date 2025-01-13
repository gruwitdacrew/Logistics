using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Account.AccountDTOs.Responses;
using Logistics.Data.Account.Models;
using Logistics.Data.Accounts.DTOs.Requests;
using Logistics.Data.Accounts.DTOs.Responses;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Documents.Models;
using Logistics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Controllers
{
    [ApiController]
    [Route("api/user/[action]")]
    public class AccountController : ControllerBase
    {
        UserService _userService;
        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> register(Role role, RegisterRequestDTO registerRequest)
        {
            return await _userService.Register(role, registerRequest);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokenResponse>> login(LoginRequestDTO loginRequest)
        {
            return await _userService.Login(loginRequest);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> logout()
        {
            var userId = User.Claims.ToList()[0].Value;
            var token = Request.Headers.Authorization.ToString().Substring(7);

            return await _userService.Logout(new Guid(userId));
        }

        [Authorize(Policy = "RefreshTokenAccess")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokenResponse>> refresh()
        {
            var userId = User.Claims.ToList()[0].Value;
            var token = Request.Headers.Authorization.ToString().Substring(7);

            return await _userService.Refresh(new Guid(userId), token);
        }

        [Authorize]
        [HttpGet]
        [Route("/api/user/role")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> getRole()
        {
            return User.Claims.ToList()[2].Value;
        }

        [Authorize(Roles = "Shipper")]
        [HttpGet]
        [Route("/api/shipper/profile")]
        [ProducesResponseType(typeof(ShipperProfileResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShipperProfileResponse>> shipperProfile()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.ShipperProfile(new Guid(userId));
        }


        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("/api/transporter/profile")]
        [ProducesResponseType(typeof(TransporterProfileResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TransporterProfileResponse>> transporterProfile()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.TransporterProfile(new Guid(userId));
        }

        [Authorize(Roles = "Shipper")]
        [HttpPut]
        [Route("/api/shipper/profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> profile(EditShipperRequestDTO editRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.EditShipper(new Guid(userId), editRequest);
        }

        [Authorize(Roles = "Transporter")]
        [HttpPut]
        [Route("/api/transporter/profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> profile(EditTransporterRequestDTO editRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.EditTransporter(new Guid(userId), editRequest);
        }

        [Route("/api/user/password/reset/request")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> resetPasswordRequest([Required] string email)
        {
            return await _userService.ResetPasswordRequest(email);
        }

        [Authorize(Policy = "ResetPassword")]
        [Route("/api/user/password/reset")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> resetPassword()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.ResetPassword(new Guid(userId));
        }

        [Authorize(Policy = "ResetPassword")]
        [Route("/api/user/password/setAfterReset")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> setPasswordAfterReset(
            [Required]
            [RegularExpression(pattern: "^.{8,}$", ErrorMessage = "Пароль должен состоять из минимум 8 символов")]
            [FromBody] string password
            )
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.SetNewPassword(new Guid(userId), null, password);
        }

        [Authorize]
        [Route("/api/user/password/set")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> setPassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.SetNewPassword(new Guid(userId), changePasswordRequest.oldPassword, changePasswordRequest.password);
        }


        [Authorize(Policy = "ApproveEmail")]
        [Route("/api/user/email/approve")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> approveEmail(bool isEmailOwner)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.ApproveEmail(new Guid(userId), isEmailOwner);
        }


        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public async Task<IActionResult> company()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.AboutCompany(new Guid(userId));
        }


        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> company(EditCompanyRequestDTO editShipperRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.EditCompany(new Guid(userId), editShipperRequest);
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> photo(IFormFile file)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.UploadPhoto(new Guid(userId), file);
        }

        [Authorize]
        [HttpGet]
        [Route("/api/user/photo")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        public async Task<ActionResult> getPhoto()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.GetPhoto(new Guid(userId));
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> photo()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.DeletePhoto(new Guid(userId));
        }


        [Authorize(Roles = "Transporter")]
        [HttpPost]
        [Route("/api/transporter/transport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> createTransport(CreateTruckRequestDTO createRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.CreateTransporterTruck(new Guid(userId), createRequest);
        }


        [Authorize(Roles = "Transporter")]
        [HttpGet]
        [Route("/api/transporter/transport")]
        [ProducesResponseType(typeof(TruckResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> getTransport()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.GetTransporterTruck(new Guid(userId));
        }


        [Authorize(Roles = "Transporter")]
        [HttpPut]
        [Route("/api/transporter/transport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> editTransport(EditTruckRequestDTO editRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.EditTransporterTruck(new Guid(userId), editRequest);
        }
    }
}

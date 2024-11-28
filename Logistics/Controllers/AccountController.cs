using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Account.AccountDTOs.Responses;
using Logistics.Data.Account.Models;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Services;
using Logistics.Services.Utils.TokenGenerator;
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
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> register([Required] Role role, RegisterRequestDTO registerRequest)
        {
            return await _userService.Register(role, registerRequest);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<TokenResponse>> login(LoginRequestDTO loginRequest)
        {
            return await _userService.Login(loginRequest);
        }

        [Authorize(Policy = "RefreshTokenAccess")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> logout()
        {
            var userId = User.Claims.ToList()[0].Value;
            var token = Request.Headers.Authorization.ToString().Substring(7);

            return await _userService.Logout(new Guid(userId), token);
        }

        [Authorize(Policy = "RefreshTokenAccess")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponse>> refresh()
        {
            var userId = User.Claims.ToList()[0].Value;
            var token = Request.Headers.Authorization.ToString().Substring(7);

            return await _userService.Refresh(new Guid(userId), token);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProfileResponse>> profile()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.Profile(new Guid(userId));
        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> profile(EditRequestDTO editRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.Edit(new Guid(userId), editRequest);
        }

        [Route("/api/user/password/reset/request")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> resetPasswordRequest([Required] string email)
        {
            return await _userService.ResetPasswordRequest(email);
        }

        [Authorize(Policy = "ResetPassword")]
        [Route("/api/user/password/reset")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> resetPassword()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.ResetPassword(new Guid(userId));
        }

        [Authorize(Policy = "ResetPassword")]
        [Route("/api/user/password/setAfterReset")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> setPasswordAfterReset(
            [Required]
            [RegularExpression(pattern: "^.{8,}$", ErrorMessage = "Пароль должен состоять из минимум 8 символов")]
            string password
            )
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.SetNewPassword(new Guid(userId), null, password);
        }

        [Authorize]
        [Route("/api/user/password/set")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> setPassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.SetNewPassword(new Guid(userId), changePasswordRequest.oldPassword, changePasswordRequest.password);
        }


        [Authorize(Policy = "ApproveEmail")]
        [Route("/api/user/email/approve")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> approveEmail(bool isEmailOwner)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.ApproveEmail(new Guid(userId), isEmailOwner);
        }


        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> company()
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.AboutCompany(new Guid(userId));
        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> company(EditCompanyRequestDTO editShipperRequest)
        {
            var userId = User.Claims.ToList()[0].Value;

            return await _userService.EditCompany(new Guid(userId), editShipperRequest);
        }
    }
}

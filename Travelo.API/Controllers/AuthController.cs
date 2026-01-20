using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Claims;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.UseCases.Auth;

namespace Travelo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register (
            [FromBody] RegisterDTO registerDTO,
            [FromServices] RegisterUseCase registerUseCase
            )
        {
            var result = await registerUseCase.ExecuteAsync(registerDTO);

            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(
            [FromBody] ConfirmEmailDTO confirmEmailDTO,
            [FromServices] ConfirmEmailUseCase confirmEmailUseCase
            )
        {
            var result = await confirmEmailUseCase.ExecuteAsync(confirmEmailDTO);

            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail(
            [FromBody] ResendConfirmEmailDTO resendConfirmationEmailDTO,
            [FromServices] ResendConfirmEmailUseCase resendConfirmationEmailUseCase
            )
        {
            var result = await resendConfirmationEmailUseCase.ExecuteAsync(resendConfirmationEmailDTO);
            return !result.Success ? BadRequest(result) : Ok(result);
        }   


        [Authorize]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword (
            [FromBody] ChangePasswordDTO changePasswordDTO,
            [FromServices] ChangePasswordUseCase changePasswordUseCase
            )
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId==null)
            {
                return Unauthorized();
            }
            var result = await changePasswordUseCase.ExecuteAsync(changePasswordDTO, userId);
            return !result.Success ? BadRequest(result) : Ok(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (
            [FromBody] LoginDTO loginDTO,
            [FromServices] LoginUseCase loginUseCase)
        {
            var result = await loginUseCase.ExecuteAsync(loginDTO);

            return !result.Success ? Unauthorized(result) : Ok(result);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword (
            [FromBody] ForgotPasswordDTO forgotPasswordDTO,
            [FromServices] ForgotPasswordUseCase forgotPasswordUseCase
            )
        {
            var result = await forgotPasswordUseCase.ExecuteAsync(forgotPasswordDTO);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword (
            [FromBody] ResetPasswordDTO resetPasswordDTO,
            [FromServices] ResetPasswordUseCase resetPasswordUseCase
            )
        {

            var result = await resetPasswordUseCase.ExecuteAsync(resetPasswordDTO);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    



        [HttpGet("Google-Login")]
        public async Task<IActionResult> GoogleLogin ()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri=Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);

        }

        [HttpGet("Google-Response")]
        public async Task<IActionResult> GoogleResponse ([FromServices] GoogleLoginUseCase googleLoginUseCase)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest("Google authentication failed");

            var token = await googleLoginUseCase.ExecuteAsync(result.Principal);

            return token==null ? BadRequest("Login failed") : Ok(new { token });
        }




    }
}



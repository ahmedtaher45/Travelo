using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.Common.Responses;
using System.Security.Claims;
using System.Security.Claims;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.Interfaces;
using Travelo.Application.UseCases.Auth;

namespace Travelo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDTO registerDTO,
            [FromServices] RegisterUseCase registerUseCase
            )
        {
            var result = await registerUseCase.ExecuteAsync(registerDTO);

            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [Authorize]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword (
            [FromBody] ChangePasswordDTO changePasswordDTO,
            [FromServices] ChangePasswordUseCase changePasswordUseCase
            )
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId==null)
            {
                return Unauthorized();
            }
            var result = await changePasswordUseCase.ExecuteAsync(changePasswordDTO, userId);
            return !result.Success ? BadRequest(result) : Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDTO loginDTO,
            [FromServices] LoginUseCase loginUseCase)
        {
            var result = await loginUseCase.ExecuteAsync(loginDTO);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDTO forgotPasswordDTO,
            [FromServices] ForgotPasswordUseCase forgotPasswordUseCase
            )
        {
            var result = await forgotPasswordUseCase.ExecuteAsync(forgotPasswordDTO);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
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
    


        [HttpGet("google-login")]
        public async Task<IActionResult> GoogleLogin() 
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);

        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse([FromServices] GoogleLoginUseCase googleLoginUseCase)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest("Google authentication failed");

            var token = await googleLoginUseCase.ExecuteAsync(result.Principal);

            if (token == null) return BadRequest("Login failed");

            return Ok(new { token });
        }




    }
}



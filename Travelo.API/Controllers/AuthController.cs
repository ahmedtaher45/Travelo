using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.Common.Responses;
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

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
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
    }


        [HttpGet("Google-Login")]
        public async Task<IActionResult> GoogleLogin() 
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);

        }

        [HttpGet("Google-Response")]
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.UseCases.Auth;

namespace Travelo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register (
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId==null)
            {
                return Unauthorized();
            }
            var result = await changePasswordUseCase.ExecuteAsync(changePasswordDTO, userId);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
    }
}



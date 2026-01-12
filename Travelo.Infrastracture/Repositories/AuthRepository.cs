using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context,IConfiguration configuration)
        {
            this.userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        
        public async Task<GenericResponse<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO.Email == null)
            {
                return GenericResponse<string>.FailureResponse("Invalid Email");
            }
            ApplicationUser? user = null;

            try
            {
                user = new ApplicationUser
                {
                    Email = registerDTO.Email,
                    UserName = registerDTO.UserName,
                    PhoneNumber = registerDTO.PhoneNumber
                };
                var result = await userManager.CreateAsync(user, registerDTO.Password!);

                if (!result.Succeeded)
                {
                    return GenericResponse<string>.FailureResponse(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                if (string.IsNullOrEmpty(user.Id))
                {
                    return GenericResponse<string>.FailureResponse("User ID is null after creation");
                }

                //await userManager.AddToRoleAsync(user, "User");

            }
            catch (Exception ex)
            {
                if (user != null)
                    await userManager.DeleteAsync(user);

                var errorMessage = ex.InnerException != null
                      ? ex.InnerException.Message
                      : ex.Message;

                    return GenericResponse<string>.FailureResponse("Error: " + errorMessage + " | StackTrace: " + ex.StackTrace);
            }

            //await SendConfirmationEmail(user, registerDTO.ClientUri!);

            return GenericResponse<string>.SuccessResponse("Check your Email for Confirmation");
        }
        public async Task<GenericResponse<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return GenericResponse<AuthResponseDTO>.FailureResponse("Invalid Email or Password");
            }

            var token = GenerateJwtToken(user);

            var authData = new AuthResponseDTO
            {
                Token = token,
                UserName = user.UserName!,
                Email = user.Email!
            };

            return GenericResponse<AuthResponseDTO>.SuccessResponse(authData, "Login Successful");
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public AuthRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
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
    }
}

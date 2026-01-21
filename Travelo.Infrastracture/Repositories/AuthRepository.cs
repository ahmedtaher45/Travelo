using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.Auth;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public AuthRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration, IEmailSender emailSender)
        {
            this.userManager = userManager;
            _context = context;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public AuthRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<GenericResponse<string>> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return GenericResponse<string>.FailureResponse("User not found");
            }
            var result = await userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
            return !result.Succeeded
                ? GenericResponse<string>.FailureResponse(string.Join(", ", result.Errors.Select(e => e.Description)))
                : GenericResponse<string>.SuccessResponse("Password changed successfully");
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

                await userManager.AddToRoleAsync(user, "User");


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

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebUtility.UrlEncode(token);
            var param = new Dictionary<string, string?>
                {
                    {"token",token },
                    {"email",registerDTO.Email!}
                };

            var confirmLink = QueryHelpers.AddQueryString(registerDTO.ClientUri!, param);


            var assembly = Assembly.Load("Travelo.Application");
            var resourceName = "Travelo.Application.Templates.Email.ConfirmEmail.html";
            using var stream = assembly.GetManifestResourceStream(resourceName);

            using var reader = new StreamReader(stream);
            var htmlTemplate = await reader.ReadToEndAsync();

            var emailBody = htmlTemplate.Replace("{{UserName}}", user.UserName)
                                         .Replace("{{ConfirmationLink}}", confirmLink)
                                         .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());


            var message = new Message(new[] { user.Email! }, "Confirm your email address ", emailBody);

            await _emailSender.SendEmailAsync(message);

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
                Token=token,
                UserName=user.UserName!,
                Email=user.Email!
            };

            return GenericResponse<AuthResponseDTO>.SuccessResponse(authData, "Login Successful");
        }
        private string GenerateJwtToken (ApplicationUser user)
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
        public async Task<GenericResponse<string>> ForgotPasswordAsync (ForgotPasswordDTO forgotPasswordDTO)
        {
            if (string.IsNullOrWhiteSpace(forgotPasswordDTO.Email))
            {
                return GenericResponse<string>.FailureResponse("Invalid Email");
            }

            try
            {
                var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email);

                if (user==null)
                {
                    return GenericResponse<string>.FailureResponse(
                        "Invalid Email"
                    );
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                token = WebUtility.UrlEncode(token);
                var param = new Dictionary<string, string?>
                {
                    {"token",token },
                    {"email",forgotPasswordDTO.Email!}
                };

                var resetLink = QueryHelpers.AddQueryString(forgotPasswordDTO.ClientUri!, param);


                var assembly = Assembly.Load("Travelo.Application");
                var resourceName = "Travelo.Application.Templates.Email.ResetPasswordEmail.html";
                using var stream = assembly.GetManifestResourceStream(resourceName);

                using var reader = new StreamReader(stream);
                var htmlTemplate = await reader.ReadToEndAsync();

                var emailBody = htmlTemplate.Replace("{{RESET_LINK}}", resetLink);


                //var message = new Message(new[] { user.Email! }, "Reset Password ", emailBody);

                //await _emailSender.SendEmailAsync(message);

                return GenericResponse<string>.SuccessResponse(
                    $"An email with a reset link has been sent."
                );
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException!=null
                    ? ex.InnerException.Message
                    : ex.Message;

                return GenericResponse<string>.FailureResponse(
                    "Error: "+errorMessage+" | StackTrace: "+ex.StackTrace
                );
            }
        }

        public async Task<GenericResponse<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user==null)
                return GenericResponse<string>.FailureResponse("User not found.");

            var Token = WebUtility.UrlDecode(resetPasswordDTO.Token);


            var result = await userManager.ResetPasswordAsync(user, Token, resetPasswordDTO.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return GenericResponse<string>.FailureResponse($"Password reset failed: {errors}");
            }

            return GenericResponse<string>.SuccessResponse("Password has been reset successfully.");
        }

        public async Task<GenericResponse<string>> ConfirmEmailAsync(ConfirmEmailDTO confirmEmailDTO)
        {
            var user = await userManager.FindByEmailAsync(confirmEmailDTO.Email);
            if (user == null)
            {
                return await Task.FromResult(GenericResponse<string>.FailureResponse("User not found."));
            }

            if (user.EmailConfirmed)
            {
                return GenericResponse<string>.SuccessResponse("Email already confirmed.");
            }

            var Token = WebUtility.UrlDecode(confirmEmailDTO.Token);

            var result = await userManager.ConfirmEmailAsync(user, Token);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return await Task.FromResult(GenericResponse<string>.FailureResponse($"Email confirmation failed:  {errors}"));


            }
            return await Task.FromResult(GenericResponse<string>.SuccessResponse("Email confirmed successfully."));
        }

        public async Task<GenericResponse<string>> ResendConfirmationEmailAsync(ResendConfirmEmailDTO resendConfirmEmailDTO)
        {
            var user = await userManager.FindByEmailAsync(resendConfirmEmailDTO.Email);

            if (user == null)
            {
                return GenericResponse<string>.FailureResponse("User not found.");
            }

            if (user.EmailConfirmed)
            {
                return GenericResponse<string>.SuccessResponse("Email already confirmed.");
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebUtility.UrlEncode(token);

            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", user.Email }
            };

            var confirmLink = QueryHelpers.AddQueryString(resendConfirmEmailDTO.ClientURI!, param);

            var assembly = Assembly.Load("Travelo.Application");
            var resourceName = "Travelo.Application.Templates.Email.ConfirmEmail.html";
            using var stream = assembly.GetManifestResourceStream(resourceName);

            using var reader = new StreamReader(stream);
            var htmlTemplate = await reader.ReadToEndAsync();

            var emailBody = htmlTemplate.Replace("{{UserName}}", user.UserName)
                                         .Replace("{{ConfirmationLink}}", confirmLink)
                                         .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());


            var message = new Message(new[] { user.Email! }, "Confirm your email address ", emailBody);

            await _emailSender.SendEmailAsync(message);


            return GenericResponse<string>.SuccessResponse("Confirmation email resent successfully.");
        }



    }
}

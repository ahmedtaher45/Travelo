using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Infrastracture.Repositories
{
    public class OAuthGoogleRepository: IOAuthGoogleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenRepository _jwtTokenRepository;

        public OAuthGoogleRepository(UserManager<ApplicationUser> userManager, IJwtTokenRepository jwtTokenRepository)
        {
            _userManager = userManager;
            _jwtTokenRepository = jwtTokenRepository;
        }

        public async Task<string> GoogleLoginAsync(ClaimsPrincipal principal)
        {
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null) return null;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = name ?? email
                };
                await _userManager.CreateAsync(user);
            }

            // Generate JWT token
            return _jwtTokenRepository.GenerateToken(user);
        }
    }
}

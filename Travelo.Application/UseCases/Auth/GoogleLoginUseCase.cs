using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Auth
{
    public class GoogleLoginUseCase
    {
        private readonly IOAuthGoogleRepository _oAuthGoogleRepository;

        public GoogleLoginUseCase(IOAuthGoogleRepository externalAuthService)
        {
            _oAuthGoogleRepository = externalAuthService;
        }

        public async Task<string> ExecuteAsync(ClaimsPrincipal principal)
        {
            var token = await _oAuthGoogleRepository.GoogleLoginAsync(principal);
            return token;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.Interfaces
{
    public interface IOAuthGoogleRepository
    {
        Task<string> GoogleLoginAsync(ClaimsPrincipal principal);

    }
}

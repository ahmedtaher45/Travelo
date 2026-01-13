using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IJwtTokenRepository
    {
        string GenerateToken(ApplicationUser user);
    }
}

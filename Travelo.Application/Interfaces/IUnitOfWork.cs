using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.Interfaces
{
    public interface IUnitOfWork
    {
         IAuthRepository Auth { get; }
         IHotelRepository Hotels { get; }

        Task<int> CompleteAsync();
        Task SaveChangesAsync();
    }
}

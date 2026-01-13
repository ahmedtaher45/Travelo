using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Common;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetFeaturedHotelsAsync(PaginationRequest request);
    }
}

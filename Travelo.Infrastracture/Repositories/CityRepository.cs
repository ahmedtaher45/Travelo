using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;
using Travelo.Infrastracture.Repositories;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository (ApplicationDbContext context)
        : base(context)
    {
    }

}

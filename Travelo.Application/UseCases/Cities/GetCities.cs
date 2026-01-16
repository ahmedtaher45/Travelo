using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Cities
{
    public class GetCities
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCities (IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

    }
}

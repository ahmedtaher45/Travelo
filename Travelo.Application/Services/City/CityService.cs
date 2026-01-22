using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.City;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;

namespace Travelo.Application.Services.City
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IFileServices _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public CityService (
            ICityRepository cityRepository,
            IFileServices fileService,
            IUnitOfWork unitOfWork)
        {
            _cityRepository=cityRepository;
            _fileService=fileService;
            _unitOfWork=unitOfWork;
        }

        public async Task<GenericResponse<IEnumerable<CityResDTO>>> GetAllCitiesAsync (int? pageNum = null,
            int? pageSize = null, string url = "")
        {
            var query = await _cityRepository.GetAll(pageNum, pageSize);
            var result = query.Select(c => new CityResDTO
            {
                Id=c.Id,
                Name=c.Name,
                Country=c.Country,
                Description=c.Description,
                ImgUrl=string.IsNullOrEmpty(c.ImgUrl) ? null : url+c.ImgUrl
            });

            return GenericResponse<IEnumerable<CityResDTO>>.SuccessResponse(result);

        }

        public async Task<GenericResponse<CityResDTO>> GetCityByIdAsync (int cityId, string url)
        {
            var city = await _cityRepository.GetById(cityId);

            if (city==null)
                return GenericResponse<CityResDTO>
                    .FailureResponse("City not found");

            var dto = new CityResDTO
            {
                Id=city.Id,
                Name=city.Name,
                Country=city.Country,
                Description=city.Description,
                ImgUrl=string.IsNullOrEmpty(city.ImgUrl) ? null : url+city.ImgUrl
            };

            return GenericResponse<CityResDTO>.SuccessResponse(dto);
        }

        public async Task<GenericResponse<string>> CreateCity (CityReqDTO cityReq)
        {
            var imageUrl = await _fileService
                .UploadFileAsync(cityReq.Img, "Cities");

            var city = new Domain.Models.Entities.City
            {
                Name=cityReq.Name,
                Country=cityReq.Country,
                Description=cityReq.Description,
                ImgUrl=imageUrl
            };

            await _cityRepository.Add(city);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("City created successfully");
        }

        public async Task<GenericResponse<string>> UpdateCity (
            int cityId,
            CityReqDTO cityReq)
        {
            var city = await _cityRepository.GetById(cityId);

            if (city==null)
                return GenericResponse<string>
                    .FailureResponse("City not found");

            city.Name=cityReq.Name;
            city.Country=cityReq.Country;
            city.Description=cityReq.Description;

            if (cityReq.Img!=null)
            {
                city.ImgUrl=await _fileService
                    .UploadFileAsync(cityReq.Img, "Cities");
            }

            _cityRepository.Update(city);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("City updated successfully");
        }

        public async Task<GenericResponse<string>> RemoveCity (int cityId)
        {
            var city = await _cityRepository.GetById(cityId);

            if (city==null)
                return GenericResponse<string>
                    .FailureResponse("City not found");

            _cityRepository.Delete(city);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("City deleted successfully");
        }
    }
}

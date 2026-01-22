using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class DeleteItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileService;

        public DeleteItemUseCase (IUnitOfWork unitOfWork, IFileServices fileService)
        {
            _unitOfWork=unitOfWork;
            _fileService=fileService;
        }
        public async Task<GenericResponse<string>> ExecuteAsync (int itemId)
        {
            var item = await _unitOfWork
                .Repository<MenuItem>()
                .GetById(itemId);

            if (item==null||item.IsDeleted)
            {
                return GenericResponse<string>
                    .FailureResponse("Item not found.");
            }


            item.IsDeleted=true;
            _unitOfWork.Repository<MenuItem>().Update(item);

            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("Item deleted successfully.");
        }
    }
}

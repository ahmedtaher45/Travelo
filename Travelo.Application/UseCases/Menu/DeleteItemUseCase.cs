using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Travelo.Application.UseCases.Menu
{
    public class DeleteItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
<<<<<<< HEAD
        private readonly IFileServices _fileService;

        public DeleteItemUseCase(IUnitOfWork unitOfWork , IFileServices fileService)
=======
        private readonly IFileService _fileService;

        public DeleteItemUseCase(IUnitOfWork unitOfWork , IFileService fileService)
>>>>>>> origin/main
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<GenericResponse<string>> ExecuteAsync(int itemId)
        {
            var item = await _unitOfWork
                .Repository<MenuItem>()
                .GetById(itemId);

            if (item == null || item.IsDeleted)
            {
                return GenericResponse<string>
                    .FailureResponse("Item not found.");
            }


            item.IsDeleted = true;
            _unitOfWork.Repository<MenuItem>().Update(item);

            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("Item deleted successfully.");
        }
    }
}

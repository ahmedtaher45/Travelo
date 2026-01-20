using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class DeleteCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GenericResponse<string>> ExecuteAsync(int categoryId)
        {
            
            var category = await _unitOfWork.Repository<MenuCategory>().GetById(categoryId);

            if (category == null || category.IsDeleted)
            {
                return GenericResponse<string>.FailureResponse("Category not found.");
            }

            category.IsDeleted = true;
            

            if (category.items != null && category.items.Any())
            {
                foreach (var item in category.items)
                {
                    item.IsDeleted = true;

                    _unitOfWork.Repository<MenuItem>().Update(item);
                }
            }

            _unitOfWork.Repository<MenuCategory>().Update(category);

            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>.SuccessResponse("Category deleted successfully.");
        }

    }
}

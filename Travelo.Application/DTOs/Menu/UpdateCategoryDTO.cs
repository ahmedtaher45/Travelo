using Microsoft.AspNetCore.Http;

namespace Travelo.Application.DTOs.Menu
{
    public class UpdateCategoryDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

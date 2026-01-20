using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Menu
{
    public class AddCategoryDTO
    {
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

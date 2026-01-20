using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Menu
{
    public class UpdateCategoryDTO
    {
        [Required]
        public int CategoryId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; } 
    }
}

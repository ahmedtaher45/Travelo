using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Travelo.Application.DTOs.City
{
    public class CityReqDTO
    {
        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [MaxLength(500)]
        [MinLength(10)]
        public string Description { get; set; }
        [Required]
        public IFormFile Img { get; set; }
    }
}

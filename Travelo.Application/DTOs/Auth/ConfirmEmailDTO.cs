using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Auth
{
    public class ConfirmEmailDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }
        
    }
}

using System.ComponentModel.DataAnnotations;

namespace Travelo.Application.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "CurrentPassword is required.")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "NewPassword is required.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "NewPassword must be between 6 and 16 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,16}$",
         ErrorMessage = "NewPassword must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; }

    }
}

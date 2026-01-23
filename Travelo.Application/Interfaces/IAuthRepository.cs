using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Restaurant;

namespace Travelo.Application.Interfaces
{
    public interface IAuthRepository
    {
    
        Task<GenericResponse<string>> RegisterAsync (RegisterDTO registerDTO);
        Task<GenericResponse<string>> ChangePasswordAsync (ChangePasswordDTO changePasswordDTO, string userId);
        Task<GenericResponse<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO);
        Task<GenericResponse<string>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
        Task<GenericResponse<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<GenericResponse<string>> ConfirmEmailAsync(ConfirmEmailDTO confirmEmailDTO);
        Task<GenericResponse<string>> ResendConfirmationEmailAsync(ResendConfirmEmailDTO resendConfirmEmailDTO);
        Task<GenericResponse<string>> AddHotel(int cityId, AddHotelDTO dto);
        Task<GenericResponse<string>> AddRestaurant(int cityId, AddRestaurantDto addRestaurantDto);
        Task<GenericResponse<string>> AddAdmin(AdminDTO adminDTO);

    }
}

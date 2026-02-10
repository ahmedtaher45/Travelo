using Microsoft.AspNetCore.Identity;
using Stripe;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;

public class UpdateUserProfileUseCase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IFileServices fileServices;

    public UpdateUserProfileUseCase (IUnitOfWork unitOfWork, IFileServices fileServices)
    {
        this.unitOfWork=unitOfWork;
        this.fileServices=fileServices;
    }

    public async Task<GenericResponse<string>> ExecuteAsync (updateUserProfileDTO dto, string userId)
    {
        var updateRepoData = new updateUserProfileDTORes
        {
            UserName = dto.UserName,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Address = dto.Address,
            Gender = dto.Gender,
            // مبدئياً الصورة والهاش فاضيين
            ProfilePictureUrl = null,
            ProfileImageHash = null
        };

        // 3. معالجة الصورة (لو اليوزر بعت صورة)
        if (dto.ProfilePictureUrl != null && dto.ProfilePictureUrl.Length > 0)
        {
            // أ) نحسب الهاش الجديد
            var newHash = await fileServices.CalculateHashAsync(dto.ProfilePictureUrl);

            // ب) نجيب الهاش القديم من الداتابيز (عن طريق الريبوزيتوري)
            // (بدل userManager.FindById اللي كانت عاملة Error)
            var oldHash = await unitOfWork.Auth.GetProfileImageHashAsync(userId);

          
            if (newHash != oldHash)
            {
                // مختلفين -> ارفع الصورة الجديدة
                var imageUrl = await fileServices.UploadFileAsync(dto.ProfilePictureUrl, "profile-pictures");

                if (string.IsNullOrEmpty(imageUrl))
                    return GenericResponse<string>.FailureResponse("Image upload failed");

                // د) نملى البيانات الجديدة عشان تروح للريبوزيتوري
                updateRepoData.ProfilePictureUrl = imageUrl;
                updateRepoData.ProfileImageHash = newHash;
            }
            else
            {
                // نفس الهاش -> تجاهل الصورة (وفرنا الرفع والتحديث)
            }
        }

        // 4. ابعت للريبوزيتوري عشان يحفظ باقي البيانات
        return await unitOfWork.Auth.UpdateUserProfileAsync(updateRepoData, userId);
    }
}

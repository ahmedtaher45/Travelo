using Microsoft.AspNetCore.Http;

namespace Travelo.Application.Services.FileService
{
    public interface IFileServices
    {
        Task<string?> UploadFileAsync (IFormFile file, string filePath);
        Task<bool> DeleteFileAsync (string fileUrl, string folderName);
        Task<string?> CalculateHashAsync(IFormFile file);
    }
}

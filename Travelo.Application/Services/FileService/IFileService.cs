using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Travelo.Application.Services.FileService
{
    public interface IFileService
    {
        Task<string?> UploadFileAsync (IFormFile file, string filePath);
        Task<bool> DeleteFileAsync (string fileUrl, string folderName);
        Task<string?> CalculateHashAsync(IFormFile file);
    }
}

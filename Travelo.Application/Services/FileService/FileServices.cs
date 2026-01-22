using Microsoft.AspNetCore.Http;

namespace Travelo.Application.Services.FileService
{
    public class FileServices : IFileServices
    {
        public Task<bool> DeleteFileAsync (string fileUrl, string folderName)
        {
            var fullPath = Path.Combine("wwwroot", folderName, fileUrl);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);

        }

        public async Task<string?> UploadFileAsync (IFormFile file, string folderName)
        {
            if (file!=null&&file.Length>0)
            {
                var fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);

                var folderPath = Path.Combine("wwwroot", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }

    }
}

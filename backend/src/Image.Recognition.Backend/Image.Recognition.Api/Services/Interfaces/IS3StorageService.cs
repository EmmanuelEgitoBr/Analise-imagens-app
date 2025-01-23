using AwsImage = Amazon.Rekognition.Model;

namespace Image.Recognition.Api.Services.Interfaces
{
    public interface IS3StorageService
    {
        Task<AwsImage.Image> GetImageAsync(string fileName);
        Task<string> UploadImageAsync(IFormFile file, string fileName);
        Task<string> ReplaceImageAsync(IFormFile file, string fileName);
        Task<string> DeleteImageAsync(string fileName);
    }
}

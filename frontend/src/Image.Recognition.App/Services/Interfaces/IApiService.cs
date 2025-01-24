using Image.Recognition.App.Models;

namespace Image.Recognition.App.Services.Interfaces
{
    public interface IApiService
    {
        Task SaveImageAsync(IFormFile file, string fileName, string storage);
        Task<ImageModel> GetImageAsync(string storage);
        Task<string> AnalyseImagesAsync(string storage);
    }
}

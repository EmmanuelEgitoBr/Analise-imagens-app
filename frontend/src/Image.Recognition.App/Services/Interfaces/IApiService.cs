using Image.Recognition.App.Models;

namespace Image.Recognition.App.Services.Interfaces
{
    public interface IApiService
    {
        Task SaveImageAsync(IFormFile file, string storage);
        Task<ImageModel> GetImageAsync(string storage);
    }
}

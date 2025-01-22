using Image.Recognition.Api.Models;

namespace Image.Recognition.Api.Services.Interfaces
{
    public interface IMongoService
    {
        Task<ImageModel> GetImageAsync(string fileName);
        Task SaveImageAsync(IFormFile file);
    }
}

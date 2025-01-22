using Image.Recognition.App.Models;

namespace Image.Recognition.App.Services.Interfaces
{
    public interface IApiService
    {
        Task SaveImage(IFormFile file, string storage);
        Task<ImageModel> GetCompleteOrder(string storage);
    }
}

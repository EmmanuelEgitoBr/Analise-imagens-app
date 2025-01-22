using Image.Recognition.App.Models;

namespace Image.Recognition.App.Services.Interfaces
{
    public interface IApiService
    {
        Task<ResponseModel> SaveImage(IFormFile file, string storage);
    }
}

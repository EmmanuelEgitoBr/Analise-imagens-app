using Microsoft.AspNetCore.Http;

namespace Image.Recognition.Domain.Interfaces
{
    public interface IImageRepository
    {
        Task SaveImageAsync(IFormFile file);
    }
}

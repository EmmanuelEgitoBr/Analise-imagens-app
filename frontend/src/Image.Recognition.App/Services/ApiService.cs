using Image.Recognition.App.Models;
using Image.Recognition.App.Services.Interfaces;
using System.Net.Http.Headers;

namespace Image.Recognition.App.Services
{
    public class ApiService : IApiService
    {
        public async Task SaveImageAsync(IFormFile file, string fileName, string storage)
        {
            string apiUrl = $"https://localhost:7175/api/v1/images/save-{storage}";
            
            using var httpClient = new HttpClient();
            using var formData = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            formData.Add(streamContent, "file", fileName);
            var response = await httpClient.PostAsync(apiUrl, formData);
        }

        public async Task<ImageModel> GetImageAsync(string storage)
        {
            using var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7175/api/v1/images/get-image-{storage}";

            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ImageModel>() ?? new ImageModel();
        }

        public async Task<string> AnalyseImagesAsync(string storage)
        {
            string apiUrl = $"https://localhost:7175/api/v1/images/analyze-image-{storage}";

            using var httpClient = new HttpClient();
            
            var response = await httpClient.PostAsync(apiUrl, new StringContent(""));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<string>() ?? string.Empty;
        }
    }
}

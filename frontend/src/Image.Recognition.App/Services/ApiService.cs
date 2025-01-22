using Image.Recognition.App.Models;
using Image.Recognition.App.Services.Interfaces;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using Image.Recognition.App.Utils;

namespace Image.Recognition.App.Services
{
    public class ApiService : IApiService
    {
        public async Task SaveImage(IFormFile file, string storage)
        {
            string apiUrl = $"https://localhost:7175/api/v1/images/save-{storage}"; // URL do endpoint
            string newFileName = FormatFile.SetFileName(file.FileName);

            using var httpClient = new HttpClient();
            using var formData = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            formData.Add(streamContent, "file", newFileName);
            var response = await httpClient.PostAsync(apiUrl, formData);
        }

        public async Task<ImageModel> GetCompleteOrder(string storage)
        {
            using var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7175/api/v1/images/get-image-{storage}";

            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ImageModel>() ?? new ImageModel();
        }
    }
}

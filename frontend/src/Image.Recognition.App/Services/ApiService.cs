using Image.Recognition.App.Models;
using Image.Recognition.App.Services.Interfaces;
using System.Net.Http;
using System;
using System.Net.Http.Headers;

namespace Image.Recognition.App.Services
{
    public class ApiService : IApiService
    {
        public async Task<ResponseModel> SaveImage(IFormFile file, string storage)
        {
            ResponseModel responseModel = new ResponseModel();
            
            string apiUrl = $"https://localhost:7175/api/v1/images/save-{storage}"; // URL do endpoint

            using var httpClient = new HttpClient();
            using var formData = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            formData.Add(streamContent, "file", file.FileName);
            var response = await httpClient.PostAsync(apiUrl, formData);

            return responseModel;
        }
    }
}

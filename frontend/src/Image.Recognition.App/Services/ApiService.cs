using Image.Recognition.App.Models;
using Image.Recognition.App.Services.Interfaces;
using System.Net.Http.Headers;

namespace Image.Recognition.App.Services
{
    public class ApiService : IApiService
    {
        public async Task<ResponseModel> SaveImage(IFormFile file)
        {
            ResponseModel responseModel = new ResponseModel();
            string imagePath = file.FileName;
            string apiUrl = "https://sua-api-endpoint.com/upload"; // URL do endpoint

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                // Lê o arquivo da imagem
                var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                var fileContent = new StreamContent(fileStream);

                // Configura o tipo do conteúdo
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                // Adiciona o arquivo ao corpo da requisição
                content.Add(fileContent, "ImageFile", Path.GetFileName(imagePath));

                try
                {
                    // Faz a requisição POST
                    var response = await client.PostAsync(apiUrl, content);

                    // Verifica se foi bem-sucedida
                    responseModel.Message = response.IsSuccessStatusCode ? "Imagem enviada com sucesso!" : $"Erro ao enviar imagem: {response.StatusCode}";
                    responseModel.Status = response.StatusCode;
                }
                catch (Exception ex)
                {
                    responseModel.Status = System.Net.HttpStatusCode.InternalServerError;
                    responseModel.Message = ex.Message;
                }
            }

            return responseModel;
        }
    }
}

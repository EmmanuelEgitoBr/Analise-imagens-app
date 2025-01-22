using Image.Recognition.App.Models;
using Image.Recognition.App.Models.Constants;
using Image.Recognition.App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Image.Recognition.App.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IApiService _apiService;

        public ImagesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveImage(IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    ViewBag.Message = "Por favor, selecione uma imagem válida.";
                    return View();
                }

                await _apiService.SaveImageAsync(imageFile, StorageMode.MongoDb);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CompareImage()
        {
            var result = await _apiService.GetImageAsync(StorageMode.MongoDb);
            var savedImage = Convert.ToBase64String(result.Data!);

            CompareImagesModel model = new CompareImagesModel
            {
                SavedImage = savedImage,
                PhotoImage = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AnalysePhoto(string photoData)
        {
            if (string.IsNullOrEmpty(photoData))
            {
                return BadRequest("Nenhuma foto foi capturada.");
            }

            // A foto é enviada como uma string em Base64, então vamos remover o prefixo 'data:image/png;base64,'
            var base64Data = photoData.Substring(photoData.IndexOf(',') + 1);
            var imageBytes = Convert.FromBase64String(base64Data);

            // Defina o caminho para salvar a imagem no servidor
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", Guid.NewGuid().ToString() + ".png");

            // Salve a imagem no servidor
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            //MANDAR PARA API A SER CONSTRUÍDA NO BACK PARA CHAMAR A AWS RECOKGINITION
            // SUGESTÃO DE ENDPOINT: https://localhost:7175/api/v1/images/analyze-image

            return Ok("Foto salva com sucesso!");
        }
    }
}

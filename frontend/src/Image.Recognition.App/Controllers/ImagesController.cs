using Image.Recognition.App.Helpers;
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

                await _apiService.SaveImageAsync(imageFile, "sourceImage.png", StorageMode.MongoDb);
                await _apiService.SaveImageAsync(imageFile, "sourceImage.png", StorageMode.S3Bucket);
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

            var imageFile = FormFileHelper.ConvertToIFormFile(imageBytes);

            await _apiService.SaveImageAsync(imageFile, imageFile.FileName, StorageMode.MongoDb);
            await _apiService.SaveImageAsync(imageFile, imageFile.FileName, StorageMode.S3Bucket);

            var comparisonResult = await _apiService.AnalyseImagesAsync(StorageMode.S3Bucket);

            return Ok("Foto salva com sucesso!");
        }
    }
}

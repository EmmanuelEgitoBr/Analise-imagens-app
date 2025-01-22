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
        public IActionResult TakeFacePhoto()
        {
            if (!ModelState.IsValid)
            { }

            return RedirectToAction("CompareImages");
        }
    }
}

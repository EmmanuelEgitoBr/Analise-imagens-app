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

        public async Task<IActionResult> SaveImage(IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    ViewBag.Message = "Por favor, selecione uma imagem válida.";
                    return View();
                }

                await _apiService.SaveImage(imageFile, StorageMode.MongoDb);
            }
            return RedirectToAction("Index");
        }
    }
}

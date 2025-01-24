using Image.Recognition.Api.Models;
using Image.Recognition.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Image.Recognition.Api.Controllers
{
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMongoService _mongoService;
        private readonly IS3StorageService _storageService;
        private readonly IRecognitionService _recognitionService;

        public ImagesController(IMongoService mongoService,
            IS3StorageService storageService,
            IRecognitionService recognitionService)
        {
            _mongoService = mongoService;
            _storageService = storageService;
            _recognitionService = recognitionService;
        }

        [HttpPost("save-mongo")]
        public async Task<IActionResult> SaveImageInMongoDb(IFormFile file)
        {
            await _mongoService.SaveImageAsync(file);

            return Ok(file.FileName);
        }

        [HttpPost("save-bucket")]
        public async Task<IActionResult> SaveImageInS3Bucket(IFormFile file)
        {
            var result = await _storageService.UploadImageAsync(file, file.FileName);

            return Ok(result);
        }

        [HttpGet("get-image-mongo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageModel>> GetImageInMongo()
        {
            var result = await _mongoService.GetImageAsync();

            return result;
        }

        [HttpPost("analyze-image-mongo")]
        public IActionResult AnalyseImagesFromMongo(byte[] photo)
        {
            var result = _recognitionService.AnalyseImageFromMongoAsync(photo);

            return Ok(result);
        }

        [HttpPost("analyze-image-bucket")]
        public async Task<ActionResult<string>> AnalyseImagesFromS3Bucket()
        {
            var result = await _recognitionService.AnalyseImageFromS3BucketAsync();

            return Ok(result);
        }
    }
}

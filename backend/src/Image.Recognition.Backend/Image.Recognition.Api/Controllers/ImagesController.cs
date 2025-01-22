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
        private readonly IRecognitionService _recognitionService;

        public ImagesController(IMongoService mongoService, IRecognitionService recognitionService)
        {
            _mongoService = mongoService;
            _recognitionService = recognitionService;
        }

        [HttpPost("save-mongo")]
        public async Task<IActionResult> SaveImageInMongoDb(IFormFile file)
        {
            await _mongoService.SaveImageAsync(file);

            return Ok(file.FileName);
        }

        [HttpPost("save-bucket")]
        public IActionResult SaveImageInS3Bucket(IFormFile file)
        {
            return Ok(file.FileName);
        }

        [HttpGet("get-image-mongo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageModel>> GetImageInMongo()
        {
            var result = await _mongoService.GetImageAsync();

            return result;
        }

        [HttpGet("get-image-bucket/{fileName}")]
        public IActionResult GetImageInS3Bucket(string fileName)
        {
            return Ok(fileName);
        }

        [HttpPost("analyze-image")]
        public IActionResult AnalyseImages(byte[] photo)
        {
            var result = _recognitionService.AnalyseImageAsync(photo);

            return Ok(result);
        }
    }
}

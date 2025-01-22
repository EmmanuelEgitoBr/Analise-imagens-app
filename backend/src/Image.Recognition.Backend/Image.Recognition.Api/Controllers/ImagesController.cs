using Image.Recognition.Api.Models;
using Image.Recognition.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Image.Recognition.Api.Controllers
{
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMongoService _mongoService;

        public ImagesController(IMongoService mongoService)
        {
            _mongoService = mongoService;
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

        [HttpPost("get-image-mongo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageModel>> GetImageInMongo(string fileName)
        {
            var result = await _mongoService.GetImageAsync(fileName);

            return result;
        }

        [HttpGet("get-image-bucket")]
        public IActionResult GetImageInS3Bucket(string fileName)
        {
            return Ok(fileName);
        }
    }
}

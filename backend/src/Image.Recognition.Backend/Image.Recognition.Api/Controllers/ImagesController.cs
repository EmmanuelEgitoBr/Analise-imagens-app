using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Image.Recognition.Api.Controllers
{
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpPost("save-mongo")]
        public IActionResult SaveImageInMongoDb(IFormFile file)
        {
            return Ok(file.FileName);
        }

        [HttpPost("save-bucket")]
        public IActionResult SaveImageInS3Bucket(IFormFile file)
        {
            return Ok(file.FileName);
        }
    }
}

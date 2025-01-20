using Microsoft.AspNetCore.Mvc;

namespace Image.Recognition.Api.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpPost("save-image")]
        public IActionResult SaveImage()
        {
            return Ok();
        }
    }
}

using System.Net;

namespace Image.Recognition.App.Models
{
    public class ResponseModel
    {
        public string Content { get; set; } = string.Empty;
        public HttpStatusCode? Status { get; set; }
        public string? Message { get; set; }
    }
}

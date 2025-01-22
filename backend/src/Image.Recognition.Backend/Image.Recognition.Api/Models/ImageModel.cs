﻿namespace Image.Recognition.Api.Models
{
    public class ImageModel
    {
        public string? Id { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
    }
}

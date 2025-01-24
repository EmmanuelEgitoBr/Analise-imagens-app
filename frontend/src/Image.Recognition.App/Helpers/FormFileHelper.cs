namespace Image.Recognition.App.Helpers
{
    public static class FormFileHelper
    {
        public static IFormFile ConvertToIFormFile(byte[] imageBytes)
        {
            // Criar um MemoryStream a partir dos bytes do arquivo
            var stream = new MemoryStream(imageBytes);

            // Criar um IFormFile usando o MemoryStream
            return new FormFile(stream, 0, imageBytes.Length, "file", "targetImage.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
        }
    }
}

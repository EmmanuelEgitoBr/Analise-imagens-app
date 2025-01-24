using Image.Recognition.Api.Models;
using Image.Recognition.Api.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Image.Recognition.Api.Services.MongoDb
{
    public class MongoService : IMongoService
    {
        private readonly IMongoCollection<ImageModel> _imageCollection;

        public MongoService(IMongoDatabase database)
        {
            _imageCollection = database.GetCollection<ImageModel>("Imagens");
        }

        public async Task<ImageModel> GetImageAsync()
        {
            return await _imageCollection.Find(image => image.FileName!.StartsWith("sourceImage")).FirstOrDefaultAsync();
        }

        public async Task SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Arquivo inválido.");
            }

            await _imageCollection.FindOneAndDeleteAsync(image => image.FileName == file.FileName);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var image = new ImageModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Data = memoryStream.ToArray()
            };

            await _imageCollection.InsertOneAsync(image);
        }

    }
}

using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Image.Recognition.Api.Services.Interfaces;

namespace Image.Recognition.Api.Services.Recognition
{
    public class RecognitionService : IRecognitionService
    {
        private readonly IAmazonRekognition _rekognitionClient;
        private readonly IMongoService _mongoService;

        public RecognitionService(IAmazonRekognition rekognitionClient, IMongoService mongoService)
        {
            _rekognitionClient = rekognitionClient;
            _mongoService = mongoService;
        }

        public async Task<string> AnalyseImageAsync(byte[] photo)
        {
            var baseImage = _mongoService.GetImageAsync().Result.Data;

            var comparisonResult = await CompareImagesAsync(photo, baseImage!);

            return "Oi";
        }

        private async Task<CompareFacesResponse> CompareImagesAsync(byte[] image1, byte[] image2)
        {
            var request = new CompareFacesRequest
            {
                SourceImage = new Amazon.Rekognition.Model.Image
                {
                    Bytes = new MemoryStream(image1)
                },
                TargetImage = new Amazon.Rekognition.Model.Image
                {
                    Bytes = new MemoryStream(image2)
                }
            };

            return await _rekognitionClient.CompareFacesAsync(request);
        }
    }
}

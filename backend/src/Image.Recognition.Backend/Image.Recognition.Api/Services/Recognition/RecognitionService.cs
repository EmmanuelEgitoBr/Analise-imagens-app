using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Image.Recognition.Api.Models;
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
            var baseImage = _mongoService.GetImageAsync().Result.Data!;

            //var image2 = Convert.FromBase64String(baseImage!);

            var comparisonResult = await CompareImagesAsync(photo, baseImage);

            return "Oi";
        }

        private async Task<CompareFacesResponse> CompareImagesAsync(byte[] sourceImage, byte[] targetImage)
        {
            var requestModel = new CompareFacesRequestModel
            {
                SourceImageBase64 = new MemoryStream(sourceImage),
                TargetImageBase64 = new MemoryStream(targetImage)
            };

            var request = new CompareFacesRequest
            {
                SourceImage = new Amazon.Rekognition.Model.Image
                {
                    Bytes = requestModel.SourceImageBase64
                },
                TargetImage = new Amazon.Rekognition.Model.Image
                {
                    Bytes = requestModel.TargetImageBase64
                }
            };

            return await _rekognitionClient.CompareFacesAsync(request);
        }
    }
}

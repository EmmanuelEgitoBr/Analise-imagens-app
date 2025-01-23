using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Image.Recognition.Api.Models;
using Image.Recognition.Api.Services.Interfaces;
using AwsImage = Amazon.Rekognition.Model;

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

        public async Task<string> AnalyseImageFromMongoAsync(byte[] photo)
        {
            var baseImage = _mongoService.GetImageAsync().Result.Data!;

            //var image2 = Convert.FromBase64String(baseImage!);

            var comparisonResult = await CompareImagesFromMongoAsync(photo, baseImage);

            return "Oi";
        }

        public Task<string> AnalyseImageFromS3BucketAsync(byte[] photo)
        {
            throw new NotImplementedException();
        }

        private async Task<CompareFacesResponse> CompareImagesFromMongoAsync(byte[] sourceImage, byte[] targetImage)
        {
            var requestModel = new CompareFacesRequestModel
            {
                SourceImageBase64 = new MemoryStream(sourceImage),
                TargetImageBase64 = new MemoryStream(targetImage)
            };

            var request = new CompareFacesRequest
            {
                SourceImage = new AwsImage.Image
                {
                    Bytes = requestModel.SourceImageBase64
                },
                TargetImage = new AwsImage.Image
                {
                    Bytes = requestModel.TargetImageBase64
                }
            };

            return await _rekognitionClient.CompareFacesAsync(request);
        }
    }
}

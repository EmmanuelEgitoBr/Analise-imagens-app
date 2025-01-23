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
        private readonly IS3StorageService _storageService;

        public RecognitionService(IAmazonRekognition rekognitionClient,
            IMongoService mongoService,
            IS3StorageService storageService)
        {
            _rekognitionClient = rekognitionClient;
            _mongoService = mongoService;
            _storageService = storageService;
        }

        public async Task<string> AnalyseImageFromMongoAsync(byte[] photo)
        {
            var baseImage = _mongoService.GetImageAsync().Result.Data!;

            var comparisonResult = await CompareImagesFromMongoAsync(photo, baseImage);

            return "Oi";
        }

        public async Task<string> AnalyseImageFromS3BucketAsync()
        {
            var comparisonResult = await CompareImagesFromS3BucketAsync();

            return "Oi";
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

        private async Task<CompareFacesResponse> CompareImagesFromS3BucketAsync()
        {
            var sourceImage = _storageService.GetImageAsync("sourceImage.png").Result;
            var targetImage = _storageService.GetImageAsync("targetImage.png").Result;

            var request = new CompareFacesRequest
            {
                SourceImage = sourceImage,
                TargetImage = targetImage,
                SimilarityThreshold = 80F
            };

            return await _rekognitionClient.CompareFacesAsync(request);
        }
    }
}

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
        private readonly string _bucketName;

        public RecognitionService(IAmazonRekognition rekognitionClient,
            IMongoService mongoService,
            IS3StorageService storageService,
            IConfiguration configuration)
        {
            _rekognitionClient = rekognitionClient;
            _mongoService = mongoService;
            _storageService = storageService;
            _bucketName = configuration["AWS:BucketName"]!;
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
            var sourceImage = GetImage("sourceImage.png");
            var targetImage = GetImage("targetImage.png");

            var request = new CompareFacesRequest
            {
                SourceImage = sourceImage,
                TargetImage = targetImage,
                SimilarityThreshold = 80F
            };

            return await _rekognitionClient.CompareFacesAsync(request);
        }

        private Amazon.Rekognition.Model.Image GetImage(string fileName)
        {
            AwsImage.S3Object s3Object = new AwsImage.S3Object
            {
                Bucket = _bucketName,
                Name = fileName
            };

            var image = new AwsImage.Image
            {
                S3Object = s3Object
            };

            return image;
        }
    }
}

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Image.Recognition.Api.Services.Interfaces;
using AwsImage = Amazon.Rekognition.Model;

namespace Image.Recognition.Api.Services.S3Storage
{
    public class S3StorageService : IS3StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3StorageService(IConfiguration configuration)
        {
            _s3Client = new AmazonS3Client(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"]));
            _bucketName = configuration["AWS:BucketName"]!;
        }

        public async Task<string> DeleteImageAsync(string fileName)
        {
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            });
            return $"Arquivo {fileName} deletado com sucesso";
        }

        public async Task<Amazon.Rekognition.Model.Image> GetImageAsync(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            using var response = await _s3Client.GetObjectAsync(request);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);

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

        public async Task<string> ReplaceImageAsync(IFormFile file, string fileName)
        {
            await DeleteImageAsync(fileName);

            return await UploadImageAsync(file, fileName);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string fileName)
        {
            using var stream = file.OpenReadStream();
            var request = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = _bucketName,
                ContentType = file.ContentType
            };

            var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(request);

            return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        }
    }
}

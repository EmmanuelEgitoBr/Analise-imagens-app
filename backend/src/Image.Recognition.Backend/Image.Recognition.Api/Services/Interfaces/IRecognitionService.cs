namespace Image.Recognition.Api.Services.Interfaces
{
    public interface IRecognitionService
    {
        Task<string> AnalyseImageFromMongoAsync(byte[] photo);
        Task<string> AnalyseImageFromS3BucketAsync();
    }
}

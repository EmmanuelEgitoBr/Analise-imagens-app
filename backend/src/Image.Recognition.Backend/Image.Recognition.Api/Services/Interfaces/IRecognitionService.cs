namespace Image.Recognition.Api.Services.Interfaces
{
    public interface IRecognitionService
    {
        Task<string> AnalyseImageAsync(byte[] photo);
    }
}

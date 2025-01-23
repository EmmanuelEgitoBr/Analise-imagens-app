namespace Image.Recognition.Api.Models
{
    public class CompareFacesRequestModel
    {


        public MemoryStream? SourceImageBase64 { get; set; }
        public MemoryStream? TargetImageBase64 { get; set; }
        public float SimilarityThreshold { get; set; } = 80.0f; // Similaridade mínima
    }
}

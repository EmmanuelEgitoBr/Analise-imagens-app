namespace Image.Recognition.Api.Models
{
    public class S3CompareFacesRequestModel
    {
        public string? SourceBucket { get; set; }
        public string? SourceImage { get; set; }
        public string? TargetBucket { get; set; }
        public string? TargetImage { get; set; }
        public float SimilarityThreshold { get; set; } = 80.0f;
    }
}

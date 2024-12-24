namespace SocialFormat.Lib.Posts;

public class CommonPostImage
{
    public byte[]? SourceBytes { get; set; }
    public string? SourceUrl { get; set; }
    public bool Uploaded { get; set; }
    public string? UploadReference { get; set; }
    public string AltText { get; set; } = null!;
}

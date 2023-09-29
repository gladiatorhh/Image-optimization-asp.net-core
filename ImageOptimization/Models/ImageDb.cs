namespace ImageOptimization.Models;

public class ImageDb
{
    public Guid Id { get; set; }

    public byte[] OriginalImage { get; set; }

    public byte[] FullScreenImage { get; set; }

    public byte[] ThumbImage { get; set; }
}

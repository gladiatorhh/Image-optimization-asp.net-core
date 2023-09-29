namespace ImageOptimization.Models;

public class Image
{
    public Guid Id { get; set; }

    public string OriginalPath { get; set; }

    public string FullscreenPath { get; set; }

    public string ThumbPath { get; set; } 
}

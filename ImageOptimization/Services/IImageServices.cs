namespace ImageOptimization.Services;

public interface IImageServices
{
    Task ProccessImages(IFormFile[] images,int fullscreenWidth,int thumbWidth);

    Task<List<string>> GetAllImageIds();

    Task<Stream> GetThumbById(Guid id);
}

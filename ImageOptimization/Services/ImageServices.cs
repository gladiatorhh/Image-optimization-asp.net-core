using ImageOptimization.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Webp;

namespace ImageOptimization.Services;

public class ImageServices : IImageServices
{
    private readonly ImageDbContext _context;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ImageServices(ImageDbContext context, IServiceScopeFactory serviceScopeFactory)
    {
        _context = context;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<List<string>> GetAllImageIds() =>
        await _context.ImagesDb.Select(i => i.Id.ToString()).ToListAsync();

    public async Task<Stream> GetThumbById(Guid id)
    {
        SqlDataReader reader = null;
        Stream result = null;


        try
        {
            var database = _context.Database;

            var dbConnection = (SqlConnection)database.GetDbConnection();

            var command = new SqlCommand("SELECT ThumbImage FROM ImagesDb WHERE Id = @id;", dbConnection);

            command.Parameters.Add(new SqlParameter("@id", id));

            dbConnection.Open();

            reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                    result = reader.GetStream(0);
            }
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            reader.Close();
        }



        return result;
    }

    public async Task ProccessImages(IFormFile[] images, int fullscreenWidth, int thumbWidth)
    {
        //int imagesCount = await _context.Images.CountAsync();


        List<Task> imagesTasks = images.Select(i => Task.Run(async () =>
        {
            using var image = await Image.LoadAsync(i.OpenReadStream());

            byte[] originalImage = await SaveImageToDb(image, image.Width);
            byte[] fullScreenImage = await SaveImageToDb(image, fullscreenWidth);
            byte[] thumbImage = await SaveImageToDb(image, thumbWidth);

            var data = _serviceScopeFactory
             .CreateScope().ServiceProvider
             .GetRequiredService<ImageDbContext>();

            data.ImagesDb.Add(new Models.ImageDb
            {
                Id = Guid.NewGuid(),
                OriginalImage = originalImage,
                FullScreenImage = fullScreenImage,
                ThumbImage = thumbImage
            });

            await data.SaveChangesAsync();

        })).ToList();

        await Task.WhenAll(imagesTasks);
    }

    private async Task<string> SaveImage(Image image, int width, int imagesCount)
    {
        const int foldersCount = 30;

        if (width > 0 && image.Width > width)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            image.Mutate(i => i.Resize(width, (width / originalWidth * originalHeight)));
        }

        string viewPath = $"Images\\ArticleImages\\{(imagesCount % foldersCount).ToString()}";
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", viewPath);
        string imageName = Guid.NewGuid().ToString() + ".webp";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        await image.SaveAsWebpAsync(Path.Combine(path, imageName), new WebpEncoder
        {
            Quality = 80
        });

        return "/" + viewPath.Replace("\\", "/") + "/" + imageName;
    }

    private async Task<byte[]> SaveImageToDb(Image image, int width)
    {
        const int foldersCount = 30;

        if (width > 0 && image.Width > width)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            image.Mutate(i => i.Resize(width, (width / originalWidth * originalHeight)));
        }

        image.Metadata.ExifProfile = null;

        var memoryStream = new MemoryStream();

        await image.SaveAsWebpAsync(memoryStream, new WebpEncoder
        {
            Quality = 80
        });

        return memoryStream.ToArray();
    }
}

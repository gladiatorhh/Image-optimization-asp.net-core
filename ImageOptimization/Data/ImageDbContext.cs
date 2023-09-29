using ImageOptimization.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageOptimization.Data;

public class ImageDbContext :DbContext
{
    public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
    {
        
    }

    public DbSet<Models.Image> Images { get; set; }

    public DbSet<ImageDb> ImagesDb { get; set; }
}

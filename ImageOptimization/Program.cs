using Azure;
using ImageOptimization.Data;
using ImageOptimization.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ImageDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ImageOptimizationConnection")));
builder.Services.AddTransient<IImageServices,ImageServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    OnPrepareResponse = options =>
//    {
//        var headers = options.Context.Response.GetTypedHeaders();

//        headers.CacheControl = new CacheControlHeaderValue
//        {
//            Public = true,
//            MaxAge = TimeSpan.FromDays(30)
//        };

//        headers.Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30));
//    }
//});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

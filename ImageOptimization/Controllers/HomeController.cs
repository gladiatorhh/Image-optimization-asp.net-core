using ImageOptimization.Models;
using ImageOptimization.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;

namespace ImageOptimization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageServices _imageServices;

        public HomeController(ILogger<HomeController> logger, IImageServices imageServices)
        {
            _logger = logger;
            _imageServices = imageServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Upload() =>
            View();

        [HttpPost]
        [RequestSizeLimit(100 * 1000 * 1000)]
        public async Task<IActionResult> Upload(IFormFile[] images)
        {
            await _imageServices.ProccessImages(images, 1000, 500);

            return View("UploadSuccess");
        }

        public async Task<IActionResult> AllImages()
        {
            List<string> imageIds = await _imageServices.GetAllImageIds();

            return View(imageIds);
        }

        public async Task<IActionResult> Thumb(Guid id)
        {
            var headers = Response.GetTypedHeaders();

            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(30)
            };

            headers.Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30));

            return File(await _imageServices.GetThumbById(id),contentType: "image/webp");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
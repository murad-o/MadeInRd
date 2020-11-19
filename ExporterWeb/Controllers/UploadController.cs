using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;
using Microsoft.AspNetCore.Hosting;

namespace ExporterWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ImageResizeService _imageResizer;

        public UploadController(IWebHostEnvironment env, ImageResizeService imageResizeService)
        {
            _env = env;
            _imageResizer = imageResizeService;
        }
        
        [HttpPost("upload-about-file")]
        public async Task<IActionResult> UploadAboutFile(IFormFile upload)
        {
            var fileName = Guid.NewGuid() + Path.GetFileName(upload.FileName);
            var directory = Path.Combine(_env.WebRootPath, "uploads", "about");
            var filePath = Path.Combine("\\uploads", "about", fileName);

            await SaveImage(upload, Path.Combine(directory, fileName));
            
            return new JsonResult(new { uploaded = 1, fileName = fileName, url = filePath });
        }
        
        [HttpPost("upload-industry-image")]
        public async Task<IActionResult> UploadIndustryImage(IFormFile upload)
        {
            var fileName = Guid.NewGuid() + Path.GetFileName(upload.FileName);
            var directory = Path.Combine(_env.WebRootPath, "uploads", "industries");
            var filePath = Path.Combine("\\uploads", "industries", fileName);

            await SaveImage(upload, Path.Combine(directory, fileName));
            
            return new JsonResult(new { uploaded = 1, fileName = fileName, url = filePath });
        }

        private async Task SaveImage(IFormFile file, string fullPath)
        {
            await using Stream stream = file.OpenReadStream();
            Bitmap image = _imageResizer.Resize(new Bitmap(stream), 1000, 1000);
            
            EncoderParameters encoderParameters = new EncoderParameters(1)
            {
                Param = {[0] = new EncoderParameter(Encoder.Quality, 80L)}
            };
            
            image.Save(fullPath, 
                ImageCodecInfo
                    .GetImageDecoders()
                    .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid), 
                encoderParameters);
        }
    }
}

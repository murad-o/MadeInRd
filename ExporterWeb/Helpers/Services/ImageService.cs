using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ExporterWeb.Helpers.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private const long QualityLevel = 80L;
        private const string JpegExtension = ".jpg";
        private readonly ImageResizeService _imageResizeService;

        public ImageService(IWebHostEnvironment appEnvironment, ImageResizeService imageResizeService)
        {
            _appEnvironment = appEnvironment;
            _imageResizeService = imageResizeService;
        }

        public string Save(ImageTypes imageType, IFormFile file)
        {
            var imageInfo = GetImageInfo(imageType);
            using Stream stream = file.OpenReadStream();
            using Bitmap image = _imageResizeService.Resize(new Bitmap(stream), imageInfo.Size.Width, imageInfo.Size.Height);

            EncoderParameters encoderParameters = new EncoderParameters(1)
            {
                Param = {[0] = new EncoderParameter(Encoder.Quality, QualityLevel)}
            };
            
            string fullName = GetFullPath(imageType, imageInfo.FileName);

            image.Save(fullName, imageInfo.ImageCodec, encoderParameters);
            return imageInfo.FileName;
        }

        private struct ImageInfo
        {
            public Size Size { get; set; }
            public ImageCodecInfo ImageCodec { get; set; }
            public string FileName { get; set; }
        }

        private static ImageInfo GetImageInfo(ImageTypes imageType) => imageType switch
        {
            ImageTypes.NewsLogo => new ImageInfo
            {
                Size = new Size(1000, 1000),
                ImageCodec = GetEncoder(ImageFormat.Jpeg),
                FileName = Guid.NewGuid() + JpegExtension,
            },
            ImageTypes.EventLogo => new ImageInfo
            {
                Size = new Size(1000, 1000),
                ImageCodec = GetEncoder(ImageFormat.Jpeg),
                FileName = Guid.NewGuid() + JpegExtension,
            },
            ImageTypes.ExporterLogo => new ImageInfo
            {
                Size = new Size(1000, 1000),
                ImageCodec = GetEncoder(ImageFormat.Jpeg),
                FileName = Guid.NewGuid() + JpegExtension,
            },
            ImageTypes.ProductLogo => new ImageInfo
            {
                Size = new Size(1000, 1000),
                ImageCodec = GetEncoder(ImageFormat.Jpeg),
                FileName = Guid.NewGuid() + JpegExtension,
            },
            ImageTypes.IndustryImage => new ImageInfo
            {
                Size = new Size(1200, 800),
                ImageCodec = GetEncoder(ImageFormat.Jpeg),
                FileName = Guid.NewGuid() + JpegExtension,
            },
            _ => throw new ArgumentException(message: "Invalid ImageType", paramName: nameof(imageType)),
        };

        // Returns wwwroot/THIS_PATH
        public static string GetWebRelativePath(ImageTypes imageType) => imageType switch
        {
            ImageTypes.NewsLogo => Path.Combine("uploads", "news"),
            ImageTypes.EventLogo => Path.Combine("uploads", "events"),
            ImageTypes.ExporterLogo => Path.Combine("uploads", "exporters"),
            ImageTypes.ProductLogo => Path.Combine("uploads", "products"),
            ImageTypes.IndustryImage => Path.Combine("uploads", "industries"),
            _ => throw new ArgumentException(message: "Invalid ImageType", paramName: nameof(imageType)),
        };

        private string GetFullPath(ImageTypes imageType, string filename)
            => Path.Combine(_appEnvironment.WebRootPath, GetWebRelativePath(imageType), filename);

        public void Delete(ImageTypes imageType, string filename)
        {
            var fullPath = GetFullPath(imageType, filename);
            File.Delete(fullPath);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo
                .GetImageDecoders()
                .FirstOrDefault(codec => codec.FormatID == format.Guid)!;
        }
    }
}

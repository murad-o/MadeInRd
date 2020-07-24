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

        public ImageService(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public string Save(ImageType imageType, IFormFile file)
        {
            var imageInfo = GetImageInfo(imageType);
            using Stream stream = file.OpenReadStream();
            using Bitmap image = Resize(new Bitmap(stream), imageInfo.Size.Width, imageInfo.Size.Height);

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, QualityLevel);
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

        private ImageInfo GetImageInfo(ImageType imageType)
        {
            string filename = Guid.NewGuid().ToString();
            return imageType switch
            {
                ImageType.NewsLogo => new ImageInfo
                {
                    Size = new Size(1000, 1000),
                    ImageCodec = GetEncoder(ImageFormat.Jpeg),
                    FileName = filename + ".jpg",
                },
                _ => throw new ArgumentException(message: "Invalid ImageType", paramName: nameof(imageType)),
            };
        }

        public string GetDirectoryPath(ImageType imageType)
        {
            string uploadDir = Path.Combine(_appEnvironment.WebRootPath, "uploads");
            return imageType switch
            {
                ImageType.NewsLogo => Path.Combine(uploadDir, "news"),
                _ => throw new ArgumentException(message: "Invalid ImageType", paramName: nameof(imageType)),
            };
        }
        private string GetFullPath(ImageType imageType, string filename)
            => Path.Combine(GetDirectoryPath(imageType), filename);

        public void Delete(ImageType imageType, string filename)
        {
            var fullPath = GetFullPath(imageType, filename);
            File.Delete(fullPath);
        }

        private Bitmap Resize(Bitmap source, int width, int height)
        {
            var scale = Math.Max((double)source.Width / width, (double)source.Height / height);

            if (scale <= 1)
                return source;

            int newWidth = (int)(source.Width / scale);
            int newHeight = (int)(source.Height / scale);
            return new Bitmap(source, newWidth, newHeight);
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo
                .GetImageDecoders()
                .FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}

using System;
using System.Drawing;

namespace ExporterWeb.Helpers.Services
{
    public class ImageResizeService
    {
        public Bitmap Resize(Bitmap source, int width, int height)
        {
            var scale = Math.Max((double)source.Width / width, (double)source.Height / height);

            if (scale <= 1)
                return source;

            var newWidth = (int)(source.Width / scale);
            var newHeight = (int)(source.Height / scale);
            return new Bitmap(source, newWidth, newHeight);
        }
    }
}
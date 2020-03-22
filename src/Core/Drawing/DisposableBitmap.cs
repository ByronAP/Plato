using System;
using System.Drawing;
using System.Drawing.Imaging;
using PlatoCore.Drawing.Abstractions;

namespace PlatoCore.Drawing
{
    
    public class DisposableBitmap : IDisposableBitmap
    {
        private Bitmap _bitmap;

        private readonly BitmapOptions _options;

        public DisposableBitmap()
        {
            _options = new BitmapOptions();
        }

        public IDisposableBitmap Configure(Action<BitmapOptions> action)
        {
            action(_options);
            return this;
        }
        
        public Bitmap Render(Action<Bitmap> renderer)
        {
            _bitmap = new Bitmap(_options.Width, _options.Height, PixelFormat.Format32bppArgb);
            renderer(_bitmap);
            return _bitmap;
        }

        public void Dispose()
        {
            _bitmap?.Dispose();
        }

    }

}

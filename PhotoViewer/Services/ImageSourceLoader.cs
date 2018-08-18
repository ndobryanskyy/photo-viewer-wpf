using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PhotoViewer.Models;
using Prism.Logging;

namespace PhotoViewer.Services
{
    internal class ImageSourceLoader : IImageSourceLoader
    {
        private readonly ILoggerFacade _loggerFacade;

        private static readonly int ConcurrencyLimit = Environment.ProcessorCount;

        private static readonly SemaphoreSlim _limitingSemaphoreSlim = new SemaphoreSlim(ConcurrencyLimit, ConcurrencyLimit);

        private readonly double _rawPixelsXScaleFactor;
        private readonly double _rawPixelsYScaleFactor;

        public ImageSourceLoader(ILoggerFacade loggerFacade, ISizingService sizingService)
        {
            _loggerFacade = loggerFacade;

            _rawPixelsXScaleFactor = sizingService.CurrentDpi.DpiX;
            _rawPixelsYScaleFactor = sizingService.CurrentDpi.DpiY;
        }

        public AsyncLoadingImageWithSize StartLoadingImage(string path, int maxDimensionSize)
        {
            FileStream imageFileStream = null;
            try
            {
                imageFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var frame = BitmapFrame.Create(imageFileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                var isVertical = frame.PixelHeight > frame.PixelWidth;
                
                Size imageSize;
                if (isVertical)
                {
                    var scaleFactor = maxDimensionSize / frame.Height;
                    imageSize = new Size(frame.Width * scaleFactor, maxDimensionSize);
                }
                else
                {
                    var scaleFactor = maxDimensionSize / frame.Width;
                    imageSize = new Size(maxDimensionSize, frame.Height * scaleFactor);
                }
                return new AsyncLoadingImageWithSize(imageSize, LoadImageAsync(imageFileStream, imageSize));
            }
            catch (Exception ex)
            {
                imageFileStream?.Close();
                imageFileStream?.Dispose();
                _loggerFacade.Log($"Loading the image failed with: {ex.Message}", Category.Exception, Priority.High);
                return new AsyncLoadingImageWithSize(Size.Empty, Task.FromResult<ImageSource>(null));
            }
        }

        private async Task<ImageSource> LoadImageAsync(Stream imageFileStream, Size desiredSizeInDip)
        {
            await _limitingSemaphoreSlim.WaitAsync();
            try
            {
                return await Task.Run(() =>
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();

                    bitmapImage.DecodePixelWidth = (int)Math.Ceiling(desiredSizeInDip.Width * _rawPixelsXScaleFactor);
                    bitmapImage.DecodePixelHeight = (int)Math.Ceiling(desiredSizeInDip.Height * _rawPixelsYScaleFactor);

                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    imageFileStream.Seek(0, SeekOrigin.Begin);
                    bitmapImage.StreamSource = imageFileStream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                });
            }
            finally
            {
                imageFileStream.Close();
                imageFileStream.Dispose();
                _limitingSemaphoreSlim.Release();
            }
        }
    }
}
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

        public AsyncLoadingThumbnail StartLoadingThumbnail(string path, double sizeLimitInDip)
        {
            FileStream imageFileStream = null;
            try
            {
                imageFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var uniformSize = ComputeUniformSizeWithLimiter(imageFileStream, sizeLimitInDip);
                return new AsyncLoadingThumbnail(uniformSize, LoadImageAsync(imageFileStream, uniformSize));
            }
            catch (Exception ex)
            {
                if (imageFileStream != null)
                {
                    imageFileStream.Close();
                    imageFileStream.Dispose();
                }
                
                _loggerFacade.Log($"Loading the image failed with: {ex.Message}", Category.Exception, Priority.High);
                return new AsyncLoadingThumbnail(Size.Empty, Task.FromResult<ImageSource>(null));
            }
        }

        public async Task<ImageSource> LoadImageAsync(string path)
        {
            await _limitingSemaphoreSlim.WaitAsync();
            try
            {
                return await Task.Run(() =>
                {
                    var bitmapImage = new BitmapImage();

                    using (var imageFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = imageFileStream;
                        bitmapImage.EndInit();
                    }

                    bitmapImage.Freeze();
                    return bitmapImage;
                });
            }
            finally
            {
                _limitingSemaphoreSlim.Release();
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
                    using (imageFileStream)
                    {
                        bitmapImage.BeginInit();

                        bitmapImage.DecodePixelWidth = (int)Math.Ceiling(desiredSizeInDip.Width * _rawPixelsXScaleFactor);
                        bitmapImage.DecodePixelHeight = (int)Math.Ceiling(desiredSizeInDip.Height * _rawPixelsYScaleFactor);

                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        imageFileStream.Seek(0, SeekOrigin.Begin);
                        bitmapImage.StreamSource = imageFileStream;
                        bitmapImage.EndInit();   
                    }

                    bitmapImage.Freeze();
                    return bitmapImage;
                });
            }
            finally
            {
                _limitingSemaphoreSlim.Release();
            }
        }

        private static Size ComputeUniformSizeWithLimiter(Stream streamSource, double sizeLimitInDip)
        {
            var frame = BitmapFrame.Create(streamSource, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);

            Size imageSize;
            var isVertical = frame.PixelHeight > frame.PixelWidth;
            
            if (isVertical)
            {
                var scaleFactor = sizeLimitInDip / frame.Height;
                imageSize = new Size(frame.Width * scaleFactor, sizeLimitInDip);
            }
            else
            {
                var scaleFactor = sizeLimitInDip / frame.Width;
                imageSize = new Size(sizeLimitInDip, frame.Height * scaleFactor);
            }

            return imageSize;
        }
    }
}
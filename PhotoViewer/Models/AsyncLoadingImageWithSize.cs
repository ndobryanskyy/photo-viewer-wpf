using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PhotoViewer.Models
{
    public class AsyncLoadingImageWithSize
    {
        public AsyncLoadingImageWithSize(Size imageSize, Task<ImageSource> loadingTask)
        {
            ImageSize = imageSize;
            ImageTask = loadingTask ?? throw new ArgumentNullException(nameof(loadingTask));
        }

        public Size ImageSize { get; }

        public Task<ImageSource> ImageTask { get; }
    }
}
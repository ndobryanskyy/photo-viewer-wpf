using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PhotoViewer.Models
{
    public class AsyncLoadingThumbnail
    {
        public AsyncLoadingThumbnail(Size imageSize, Task<ImageSource> loadingTask)
        {
            ImageSize = imageSize;
            ThumbnailTask = loadingTask ?? throw new ArgumentNullException(nameof(loadingTask));
        }

        public Size ImageSize { get; }

        public Task<ImageSource> ThumbnailTask { get; }
    }
}
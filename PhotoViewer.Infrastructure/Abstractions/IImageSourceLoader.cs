using System.Threading.Tasks;
using System.Windows.Media;
using PhotoViewer.Infrastructure.Models;

namespace PhotoViewer.Infrastructure
{
    public interface IImageSourceLoader
    {
        AsyncLoadingThumbnail StartLoadingThumbnail(string path, double sizeLimitInDip);

        Task<ImageSource> LoadImageAsync(string path);
    }
}
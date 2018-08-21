using System.Threading.Tasks;
using System.Windows.Media;
using PhotoViewer.Models;

namespace PhotoViewer.Services
{
    public interface IImageSourceLoader
    {
        AsyncLoadingThumbnail StartLoadingThumbnail(string path, double sizeLimitInDip);

        Task<ImageSource> LoadImageAsync(string path);
    }
}
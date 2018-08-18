using PhotoViewer.Models;

namespace PhotoViewer.Services
{
    public interface IImageSourceLoader
    {
        AsyncLoadingImageWithSize StartLoadingImage(string path, int scaleToHeight = 0);
    }
}
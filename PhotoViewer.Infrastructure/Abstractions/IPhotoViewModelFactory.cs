using PhotoViewer.Infrastructure.ViewModels;

namespace PhotoViewer.Infrastructure
{
    public interface IPhotoViewModelFactory
    {
        PhotoViewModel Create(int index, string path);
    }
}
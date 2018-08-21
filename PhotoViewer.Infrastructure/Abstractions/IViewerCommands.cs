using System.Windows.Input;
using PhotoViewer.Infrastructure.ViewModels;
using Prism.Commands;

namespace PhotoViewer.Infrastructure
{
    public interface IViewerCommands
    {
        DelegateCommand<PhotoViewModel> OpenPhotoCommand { get; }

        ICommand GoToGalleryCommand { get; }
    }
}
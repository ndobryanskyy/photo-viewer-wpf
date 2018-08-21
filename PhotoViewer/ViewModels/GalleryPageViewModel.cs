using System.Collections.ObjectModel;
using System.Windows.Input;
using PhotoViewer.Models.EventArgs;
using PhotoViewer.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace PhotoViewer.ViewModels
{
    public class GalleryPageViewModel : BindableBase
    {
        private readonly IPhotosService _photosService;
        
        public GalleryPageViewModel(IPhotosService photosService)
        {
            _photosService = photosService;

            ImagesDroppedCommand = new DelegateCommand<ImagesDroppedEventArgs>(OnImagesDropped);

            Photos = _photosService.Photos;
        }

        public ICommand ImagesDroppedCommand { get; }

        public ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }

        private void OnImagesDropped(ImagesDroppedEventArgs e)
        {
            _photosService.AddPhotos(e.FilePaths);
        }
    }
}

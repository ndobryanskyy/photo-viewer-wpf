using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PhotoViewer.Models.EventArgs;
using PhotoViewer.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PhotoViewer.ViewModels
{
    public class GalleryPageViewModel : BindableBase
    {
        private const int ThumbnailHeight = 256;

        private readonly IImageSourceLoader _imageSourceLoader;
        private readonly IRegionManager _regionManager;

        public GalleryPageViewModel(IImageSourceLoader imageSourceLoader,IRegionManager regionManager)
        {
            _imageSourceLoader = imageSourceLoader;
            _regionManager = regionManager;
            ImagesDroppedCommand = new DelegateCommand<ImagesDroppedEventArgs>(OnImagesDropped);
            OpenPhotoCommand = new DelegateCommand<PhotoViewModel>(OnOpenPhoto);
        }

        public ICommand ImagesDroppedCommand { get; }

        public DelegateCommand<PhotoViewModel> OpenPhotoCommand { get; }

        public ObservableCollection<PhotoViewModel> Photos { get; } = new ObservableCollection<PhotoViewModel>();

        private void OnImagesDropped(ImagesDroppedEventArgs e)
        {
            foreach (var imagePath in e.FilePaths)
            {
                var loadingImageWithSize = _imageSourceLoader.StartLoadingImage(imagePath, ThumbnailHeight);
                var photo = new PhotoViewModel(loadingImageWithSize.ImageSize, OpenPhotoCommand);
                loadingImageWithSize.ImageTask.ContinueWith(imageTask =>
                {
                    if (!imageTask.IsFaulted)
                    {
                        Application.Current.Dispatcher.InvokeAsync(() => { photo.Image = imageTask.Result; }, DispatcherPriority.Normal);
                    }
                });

                Photos.Add(photo);
            }
        }

        private void OnOpenPhoto(PhotoViewModel photo)
        {
        }
    }
}

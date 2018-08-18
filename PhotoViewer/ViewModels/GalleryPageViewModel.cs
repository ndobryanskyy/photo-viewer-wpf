using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PhotoViewer.Models.EventArgs;
using PhotoViewer.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace PhotoViewer.ViewModels
{
    public class GalleryPageViewModel : BindableBase
    {
        private const int ThumbnailHeight = 256;

        private readonly IImageSourceLoader _imageSourceLoader;

        public GalleryPageViewModel(IImageSourceLoader imageSourceLoader)
        {
            _imageSourceLoader = imageSourceLoader;
            ImagesDroppedCommand = new DelegateCommand<ImagesDroppedEventArgs>(OnImagesDropped);
        }

        public ICommand ImagesDroppedCommand { get; }

        public ObservableCollection<PhotoViewModel> Photos { get; set; } = new ObservableCollection<PhotoViewModel>();

        private void OnImagesDropped(ImagesDroppedEventArgs e)
        {
            foreach (var imagePath in e.Paths)
            {
                var loadingImageWithSize = _imageSourceLoader.StartLoadingImage(imagePath, ThumbnailHeight);
                var photo = new PhotoViewModel(loadingImageWithSize.ImageSize);
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
    }
}

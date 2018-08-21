using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PhotoViewer.Services;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;

namespace PhotoViewer.ViewModels
{
    public class PhotoViewModel : BindableBase
    {
        private readonly ILoggerFacade _loggerFacade;
        private readonly IImageSourceLoader _imageSourceLoader;

        private double _thumbnailWidth;
        private double _thumbnailHeight;
        private ImageSource _thumbnail;
        private ImageSource _image;

        private bool _isImageBlurred;
        private bool _isThumbnailLoaded;
        private bool _isImageLoaded;

        public PhotoViewModel(
            ILoggerFacade loggerFacade,
            IImageSourceLoader imageSourceLoader,
            IViewerCommands viewerCommands,
            int index,
            string path)
        {
            _loggerFacade = loggerFacade;
            _imageSourceLoader = imageSourceLoader;
            
            Path = path;
            Index = index;

            OpenPhotoCommand = viewerCommands.OpenPhotoCommand;
            LoadThumbnailCommand = new DelegateCommand(StartLoadingThumbnail);
        }

        public int Index { get; }

        public string Path { get; }

        public DelegateCommand<PhotoViewModel> OpenPhotoCommand { get; }

        public ICommand LoadThumbnailCommand { get; }

        public ImageSource Thumbnail
        {
            get => _thumbnail;
            private set
            {
                if (SetProperty(ref _thumbnail, value))
                {
                    RaisePropertyChanged(nameof(Image));
                }
            }
        }

        public ImageSource Image
        {
            get => _image ?? Thumbnail;
            set => SetProperty(ref _image, value);
        }

        public bool IsImageBlurred
        {
            get => _isImageBlurred;
            set => SetProperty(ref _isImageBlurred, value);
        }

        public double ThumbnailWidth
        {
            get => _thumbnailWidth;
            private set => SetProperty(ref _thumbnailWidth, value);
        }

        public double ThumbnailHeight
        {
            get => _thumbnailHeight;
            private set => SetProperty(ref _thumbnailHeight, value);
        }

        public async void StartLoadingThumbnail()
        {
            if (!_isThumbnailLoaded)
            {
                _isThumbnailLoaded = true;
                await LoadThumbnailAsync();
            }
        }

        public void StartLoadingImage()
        {
            if (!_isImageLoaded)
            {
                _isImageLoaded = true;
                _imageSourceLoader.LoadImageAsync(Path)
                    .ContinueWith(imageTask => 
                    { 
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            if (_isImageLoaded)
                            {
                                Image = imageTask.Result;
                            }
                        });
                    });
            }
        }

        public void UnloadImage()
        {
            _isImageLoaded = false;
            Image = null;
        }

        private async Task LoadThumbnailAsync()
        {
            var asyncLoadingImageWithSize = _imageSourceLoader.StartLoadingThumbnail(Path, 256);
                
            ThumbnailWidth = asyncLoadingImageWithSize.ImageSize.Width;
            ThumbnailHeight = asyncLoadingImageWithSize.ImageSize.Height;

            var imageSource = await asyncLoadingImageWithSize.ThumbnailTask;
            Thumbnail = imageSource;
        }
    }
}
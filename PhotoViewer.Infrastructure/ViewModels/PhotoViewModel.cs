using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;

namespace PhotoViewer.Infrastructure.ViewModels
{
    public class PhotoViewModel : BindableBase, IEquatable<string>
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly IImageSourceLoader _imageSourceLoader;

        private readonly string _filePath;
        private readonly double _thumbnailSizeLimit;

        private double _thumbnailWidth;
        private double _thumbnailHeight;
        private ImageSource _thumbnail;
        private ImageSource _image;

        private bool _isImageBlurred;
        private bool _isThumbnailLoaded;
        private bool _isImageLoaded;

        public PhotoViewModel(
            IConfigProvider configProvider,
            IDispatcherService dispatcherService,
            IImageSourceLoader imageSourceLoader,
            IViewerCommands viewerCommands,
            int index,
            string filePath)
        {
            _dispatcherService = dispatcherService;
            _imageSourceLoader = imageSourceLoader;
            _thumbnailSizeLimit = configProvider.ThumbnailSizeLimit;
            
            _filePath = filePath;
            DisplayName = Path.GetFileNameWithoutExtension(_filePath);

            Index = index;

            OpenPhotoCommand = viewerCommands.OpenPhotoCommand;
            LoadThumbnailCommand = new DelegateCommand(StartLoadingThumbnail);
        }

        public int Index { get; }

        public string DisplayName { get; }

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
                _imageSourceLoader.LoadImageAsync(_filePath)
                    .ContinueWith(imageTask => 
                    {
                        _dispatcherService.ExecuteOnUIThreadAsync(() =>
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
            var asyncLoadingImageWithSize = _imageSourceLoader.StartLoadingThumbnail(_filePath, _thumbnailSizeLimit);
                
            ThumbnailWidth = asyncLoadingImageWithSize.ImageSize.Width;
            ThumbnailHeight = asyncLoadingImageWithSize.ImageSize.Height;

            var imageSource = await asyncLoadingImageWithSize.ThumbnailTask;
            Thumbnail = imageSource;
        }

        public bool Equals(string filePath) => _filePath.Equals(filePath, StringComparison.InvariantCultureIgnoreCase);
    }
}
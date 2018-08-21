using System.Windows.Input;
using PhotoViewer.Services;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;

namespace PhotoViewer.ViewModels
{
    public class CarouselPageViewModel : BindableBase, INavigationAware
    {
        private readonly ILoggerFacade _loggerFacade;
        private readonly IViewerCommands _viewerCommands;
        private readonly IPhotosService _photosService;

        private PhotoViewModel _prevPhoto;
        private PhotoViewModel _currentPhoto;
        private PhotoViewModel _nextPhoto;

        public CarouselPageViewModel(
            ILoggerFacade loggerFacade,
            IViewerCommands viewerCommands,
            IPhotosService photosService)
        {
            _loggerFacade = loggerFacade;
            _viewerCommands = viewerCommands;
            _photosService = photosService;

            BackToGalleryCommand = _viewerCommands.GoToGalleryCommand;
            NextPhotoCommand = new DelegateCommand(OnNextPhoto, () => NextPhoto != null)
                .ObservesProperty(() => NextPhoto);

            PrevPhotoCommand = new DelegateCommand(OnPrevPhoto, () => PrevPhoto != null)
                .ObservesProperty(() => PrevPhoto);
        }

        public ICommand BackToGalleryCommand { get; }

        public ICommand NextPhotoCommand { get; }

        public ICommand PrevPhotoCommand { get; }

        public PhotoViewModel CurrentPhoto
        {
            get => _currentPhoto;
            private set
            {
                if (SetProperty(ref _currentPhoto, value) &&
                    CurrentPhoto != null)
                {
                    CurrentPhoto.StartLoadingThumbnail();
                    CurrentPhoto.StartLoadingImage();
                }
            } 
        }

        private PhotoViewModel NextPhoto
        {
            get => _nextPhoto;
            set
            {
                if (SetProperty(ref _nextPhoto, value))
                {
                    NextPhoto?.StartLoadingThumbnail();
                }
            }
        }

        private PhotoViewModel PrevPhoto
        {
            get => _prevPhoto;
            set
            {
                if (SetProperty(ref _prevPhoto, value))
                {
                    PrevPhoto?.StartLoadingThumbnail();
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters[Constants.NaigationParameterKeys.PhotoIndex] is int photoIndex
                && _photosService.TryGetPhotoByIndex(photoIndex, out var photo))
            {
                CurrentPhoto = photo;
                NextPhoto = GetNextPhoto(CurrentPhoto);
                PrevPhoto = GetPrevPhoto(CurrentPhoto);
            }
            else
            {
                _loggerFacade.Log($"Failed to handle navigation parameters: {navigationContext.Parameters}", Category.Exception, Priority.High);
                _viewerCommands.GoToGalleryCommand.Execute(null);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            CurrentPhoto?.UnloadImage();
            PrevPhoto?.UnloadImage();
            NextPhoto?.UnloadImage();

            CurrentPhoto = null;
            NextPhoto = null;
            PrevPhoto = null;
        }

        private void OnNextPhoto()
        {
            PrevPhoto?.UnloadImage();
            PrevPhoto = CurrentPhoto;
            CurrentPhoto = NextPhoto;
            NextPhoto = GetNextPhoto(CurrentPhoto);
        }

        private void OnPrevPhoto()
        {
            NextPhoto?.UnloadImage();
            NextPhoto = CurrentPhoto;
            CurrentPhoto = PrevPhoto;
            PrevPhoto = GetPrevPhoto(CurrentPhoto);
        }

        private PhotoViewModel GetNextPhoto(PhotoViewModel photo)
        {
            return _photosService.TryGetPhotoByIndex(photo.Index + 1, out var nextPhoto)
                ? nextPhoto
                : null;
        }

        private PhotoViewModel GetPrevPhoto(PhotoViewModel photo)
        {
            return _photosService.TryGetPhotoByIndex(photo.Index - 1, out var prevPhoto)
                ? prevPhoto
                : null;
        }
    }
}

using System;
using System.Windows.Input;
using PhotoViewer.ViewModels;
using Prism.Commands;
using Prism.Regions;

namespace PhotoViewer.Services
{
    public interface IViewerCommands
    {
        DelegateCommand<PhotoViewModel> OpenPhotoCommand { get; }

        ICommand GoToGalleryCommand { get; }
    }

    internal class ViewerCommands : IViewerCommands
    {
        private readonly IRegionManager _regionManager;

        public ViewerCommands(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            OpenPhotoCommand = new DelegateCommand<PhotoViewModel>(OnOpenPhoto);
            GoToGalleryCommand = new DelegateCommand(OnGoToGallery);
        }

        public DelegateCommand<PhotoViewModel> OpenPhotoCommand { get; }

        public ICommand GoToGalleryCommand { get; }

        private void OnOpenPhoto(PhotoViewModel photo)
        {
            _regionManager.RequestNavigate(Constants.Regions.Root, 
                new Uri(Constants.Pages.CarouselPage, UriKind.Relative),
                new NavigationParameters
                {
                    {Constants.NaigationParameterKeys.PhotoIndex, photo.Index}
                });   
        }

        private void OnGoToGallery()
        {
            _regionManager.RequestNavigate(Constants.Regions.Root, Constants.Pages.GalleryPage);
        }
    }
}
using PhotoViewer.Infrastructure;
using PhotoViewer.Infrastructure.ViewModels;
using Prism.Logging;

namespace PhotoViewer.Services
{
    internal class PhotoViewModelFactory : IPhotoViewModelFactory
    {
        private readonly IConfigProvider _configProvider;
        private readonly IDispatcherService _dispatcherService;
        private readonly IImageSourceLoader _imageSourceLoader;
        private readonly IViewerCommands _viewerCommands;

        public PhotoViewModelFactory(
            IConfigProvider configProvider,
            IDispatcherService dispatcherService,
            IImageSourceLoader imageSourceLoader,
            IViewerCommands viewerCommands)
        {
            _configProvider = configProvider;
            _dispatcherService = dispatcherService;
            _imageSourceLoader = imageSourceLoader;
            _viewerCommands = viewerCommands;
        }

        public PhotoViewModel Create(int index, string path)
            => new PhotoViewModel(_configProvider, _dispatcherService, _imageSourceLoader, _viewerCommands, index, path);
    }
}
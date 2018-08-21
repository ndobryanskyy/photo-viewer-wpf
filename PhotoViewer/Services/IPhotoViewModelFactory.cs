using PhotoViewer.ViewModels;
using Prism.Logging;

namespace PhotoViewer.Services
{
    public interface IPhotoViewModelFactory
    {
        PhotoViewModel Create(int index, string path);
    }

    internal class PhotoViewModelFactory : IPhotoViewModelFactory
    {
        private readonly ILoggerFacade _loggerFacade;
        private readonly IImageSourceLoader _imageSourceLoader;
        private readonly IViewerCommands _viewerCommands;

        public PhotoViewModelFactory(
            ILoggerFacade loggerFacade,
            IImageSourceLoader imageSourceLoader,
            IViewerCommands viewerCommands)
        {
            _loggerFacade = loggerFacade;
            _imageSourceLoader = imageSourceLoader;
            _viewerCommands = viewerCommands;
        }

        public PhotoViewModel Create(int index, string path)
            => new PhotoViewModel(_loggerFacade, _imageSourceLoader, _viewerCommands, index, path);
    }
}
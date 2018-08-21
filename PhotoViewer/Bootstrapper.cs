using PhotoViewer.Views;
using System.Windows;
using DryIoc;
using PhotoViewer.Infrastructure;
using PhotoViewer.Services;
using Prism.DryIoc;

namespace PhotoViewer
{
    class Bootstrapper : DryIocBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterTypeForNavigation<GalleryPage>(Constants.Pages.GalleryPage);
            Container.RegisterTypeForNavigation<CarouselPage>(Constants.Pages.CarouselPage);
            
            Container.Register<IImageSourceLoader, ImageSourceLoader>(new SingletonReuse());
            Container.Register<ISizingService, SizingService>(new SingletonReuse());
            Container.Register<IViewerCommands, ViewerCommands>(new SingletonReuse());
            Container.Register<IPhotoViewModelFactory, PhotoViewModelFactory>(new SingletonReuse());
            Container.Register<IPhotosStore, PhotosStore>(new SingletonReuse());
            Container.Register<IConfigProvider, ConfigProvider>(new SingletonReuse());
            Container.Register<IDispatcherService, DispatcherService>(new SingletonReuse());
        }
    }
}

using PhotoViewer.Views;
using System.Windows;
using Prism.Modularity;
using DryIoc;
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

            Container.RegisterTypeForNavigation<GalleryPage>(Constants.Pages.Gallery);
            Container.Register<IImageSourceLoader, ImageSourceLoader>(new SingletonReuse());
            Container.Register<ISizingService, SizingService>(new SingletonReuse());
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            //moduleCatalog.AddModule(typeof(YOUR_MODULE));
        }
    }
}

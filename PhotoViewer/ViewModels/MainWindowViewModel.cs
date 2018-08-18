using PhotoViewer.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace PhotoViewer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {   
        private string _title = "Photo Viewer++";

        public MainWindowViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(Constants.Regions.Root, typeof(GalleryPage));
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}

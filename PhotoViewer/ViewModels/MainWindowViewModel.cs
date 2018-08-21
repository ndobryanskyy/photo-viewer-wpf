using PhotoViewer.Infrastructure;
using PhotoViewer.Infrastructure.Events;
using PhotoViewer.Views;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PhotoViewer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title;

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IConfigProvider configProvider)
        {
            Title = configProvider.AppName;
            regionManager.RegisterViewWithRegion(Constants.Regions.Root, typeof(GalleryPage));

            eventAggregator
                .GetEvent<ApplicationTitleChangedEvent>()
                .Subscribe(OnTitleChanged, ThreadOption.UIThread);
        }

        private void OnTitleChanged(string title)
        {
            Title = title;
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}

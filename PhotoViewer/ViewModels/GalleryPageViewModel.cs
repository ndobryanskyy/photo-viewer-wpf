using System.Collections.ObjectModel;
using System.Windows.Input;
using PhotoViewer.Infrastructure;
using PhotoViewer.Infrastructure.Events;
using PhotoViewer.Infrastructure.Models.EventArgs;
using PhotoViewer.Infrastructure.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PhotoViewer.ViewModels
{
    public class GalleryPageViewModel : BindableBase, INavigationAware
    {
        private readonly IPhotosStore _photosStore;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConfigProvider _configProvider;

        public GalleryPageViewModel(IPhotosStore photosStore, IEventAggregator eventAggregator, IConfigProvider configProvider)
        {
            _photosStore = photosStore;
            _eventAggregator = eventAggregator;
            _configProvider = configProvider;

            ImagesDroppedCommand = new DelegateCommand<ImagesDroppedEventArgs>(OnImagesDropped);

            Photos = _photosStore.Photos;
        }

        public ICommand ImagesDroppedCommand { get; }

        public ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }

        private void OnImagesDropped(ImagesDroppedEventArgs e)
        {
            _photosStore.AddPhotos(e.FilePaths);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<ApplicationTitleChangedEvent>()
                .Publish(_configProvider.AppName);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // No Op
        }
    }
}

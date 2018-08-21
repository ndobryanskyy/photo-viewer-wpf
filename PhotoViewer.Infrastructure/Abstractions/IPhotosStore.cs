using System.Collections.Generic;
using System.Collections.ObjectModel;
using PhotoViewer.Infrastructure.ViewModels;

namespace PhotoViewer.Infrastructure
{
    public interface IPhotosStore
    {
        ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }

        void AddPhotos(IEnumerable<string> filePaths);

        bool TryGetPhotoByIndex(int index, out PhotoViewModel photoViewModel);
    }
}
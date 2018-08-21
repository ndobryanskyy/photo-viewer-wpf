using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PhotoViewer.Infrastructure;
using PhotoViewer.Infrastructure.ViewModels;

namespace PhotoViewer.Services
{
    internal class PhotosStore : IPhotosStore
    {
        private readonly IPhotoViewModelFactory _photoViewModelFactory;
        private readonly ObservableCollection<PhotoViewModel> _photos;

        public PhotosStore(IPhotoViewModelFactory photoViewModelFactory)
        {
            _photoViewModelFactory = photoViewModelFactory;

            _photos = new ObservableCollection<PhotoViewModel>(); 
            Photos = new ReadOnlyObservableCollection<PhotoViewModel>(_photos);
        }

        public ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }
        
        public void AddPhotos(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!_photos.Any(x => x.Equals(filePath)))
                {
                    _photos.Add(_photoViewModelFactory.Create(_photos.Count, filePath));
                }
            }
        }

        public bool TryGetPhotoByIndex(int index, out PhotoViewModel photoViewModel)
        {
            photoViewModel = null;

            if (index < 0 || index >= _photos.Count)
            {
                return false;
            }

            photoViewModel = _photos[index];
            return true;
        }
    }
}
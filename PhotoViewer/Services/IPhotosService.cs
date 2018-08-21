using System.Collections.Generic;
using System.Collections.ObjectModel;
using PhotoViewer.ViewModels;

namespace PhotoViewer.Services
{
    public interface IPhotosService
    {
        ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }

        void AddPhotos(ICollection<string> filePaths);

        bool TryGetPhotoByIndex(int index, out PhotoViewModel photoViewModel);
    }

    internal class PhotosService : IPhotosService
    {
        private readonly IPhotoViewModelFactory _photoViewModelFactory;
        private readonly ObservableCollection<PhotoViewModel> _photos;

        public PhotosService(IPhotoViewModelFactory photoViewModelFactory)
        {
            _photoViewModelFactory = photoViewModelFactory;

            _photos = new ObservableCollection<PhotoViewModel>(); 
            Photos = new ReadOnlyObservableCollection<PhotoViewModel>(_photos);
        }

        public ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }
        
        public void AddPhotos(ICollection<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                _photos.Add(_photoViewModelFactory.Create(_photos.Count, filePath));
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
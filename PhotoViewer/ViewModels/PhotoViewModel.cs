using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;

namespace PhotoViewer.ViewModels
{
    public class PhotoViewModel : BindableBase
    {
        private ImageSource _image;

        public PhotoViewModel(Size imageSize, DelegateCommand<PhotoViewModel> openPhotoCommand)
        {
            Width = (int)imageSize.Width;
            Height = (int)imageSize.Height;
            OpenPhotoCommand = openPhotoCommand;
        }

        public ICommand OpenPhotoCommand { get; }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public int Width { get; }

        public int Height { get; }
    }
}
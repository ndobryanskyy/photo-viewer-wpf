using System.Windows;
using System.Windows.Media;
using Prism.Mvvm;

namespace PhotoViewer.ViewModels
{
    public class PhotoViewModel : BindableBase
    {
        private ImageSource _image;

        public PhotoViewModel(Size imageSize)
        {
            Width = (int)imageSize.Width;
            Height = (int)imageSize.Height;
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public int Width { get; }

        public int Height { get; }
    }
}
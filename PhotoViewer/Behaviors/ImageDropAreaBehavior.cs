using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using PhotoViewer.Models.EventArgs;

namespace PhotoViewer.Behaviors
{
    public class ImageDropAreaBehavior : Behavior<UIElement>
    {
        private static readonly string[] SupportedDropFileFormats = {
            ".JPG",
            ".PNG",
            ".BMP"
        };

        public static readonly DependencyProperty ImagesDroppedCommandProperty = DependencyProperty.Register(
            nameof(ImagesDroppedCommand),
            typeof(ICommand),
            typeof(ImageDropAreaBehavior),
            new PropertyMetadata(null));

        public ICommand ImagesDroppedCommand
        {
            get => (ICommand) GetValue(ImagesDroppedCommandProperty);
            set => SetValue(ImagesDroppedCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Drop -= OnDrop;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var imageFiles = files.Where(filePath =>
                {
                    var extension = Path.GetExtension(filePath);
                    return SupportedDropFileFormats.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
                });

                ImagesDroppedCommand?.Execute(new ImagesDroppedEventArgs(imageFiles));
            }
        }
    }
}
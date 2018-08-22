using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
using PhotoViewer.Infrastructure.Controls;
using PhotoViewer.Infrastructure.Models.EventArgs;

namespace PhotoViewer.Infrastructure.Behaviors
{
    public class ImageDropAreaBehavior : Behavior<FrameworkElement>
    {
        private static readonly string[] SupportedDropFileFormats = {
            ".JPG",
            ".JPEG",
            ".JPE",
            ".JFIF",
            ".PNG",
            ".BMP",
            ".DIB"
        };

        public static readonly DependencyProperty ImagesDroppedCommandProperty = DependencyProperty.Register(
            nameof(ImagesDroppedCommand),
            typeof(ICommand),
            typeof(ImageDropAreaBehavior),
            new PropertyMetadata(null));

        private DropHereBanner _dropHereBanner;
        private AdornerContainer _dropAdorner;
        private Window _window;

        public ICommand ImagesDroppedCommand
        {
            get => (ICommand) GetValue(ImagesDroppedCommandProperty);
            set => SetValue(ImagesDroppedCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _dropHereBanner = new DropHereBanner();

            _dropAdorner = new AdornerContainer(AssociatedObject)
            {
                Child = _dropHereBanner
            };

            AssociatedObject.AllowDrop = true;
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Drop += OnDrop;
            AssociatedObject.DragEnter += OnDragEnter;
            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.DragLeave += OnDragLeave;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _window = Window.GetWindow(AssociatedObject);
            AdornerLayer.GetAdornerLayer(AssociatedObject).Add(_dropAdorner);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Drop -= OnDrop;
            AssociatedObject.DragEnter -= OnDragEnter;
            AssociatedObject.DragLeave -= OnDragLeave;
            AssociatedObject.DragOver -= OnDragOver;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            _window.Activate();
            _dropHereBanner.Show();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            _dropHereBanner.Hide();
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            _dropHereBanner.Hide();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
                {
                    var imageFiles = files.Where(filePath =>
                    {
                        var extension = Path.GetExtension(filePath);
                        return SupportedDropFileFormats.Any(x => x.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
                    });

                    ImagesDroppedCommand?.Execute(new ImagesDroppedEventArgs(imageFiles));
                }
                else
                {
                    throw new InvalidOperationException($"Expected {nameof(e.Data)} to contain string[], actual: {e.Data.GetData(DataFormats.FileDrop)?.GetType().FullName ?? "NULL"}");
                }
            }
        }
    }
}
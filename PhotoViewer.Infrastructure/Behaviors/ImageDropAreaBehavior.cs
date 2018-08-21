using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            ".PNG",
            ".BMP"
        };

        public static readonly DependencyProperty ImagesDroppedCommandProperty = DependencyProperty.Register(
            nameof(ImagesDroppedCommand),
            typeof(ICommand),
            typeof(ImageDropAreaBehavior),
            new PropertyMetadata(null));

        private DropHereBanner _dropHereBanner;
        private AdornerContainer _dropAdorner;

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
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.Drop += OnDrop;
            AssociatedObject.DragEnter += AssociatedObjectOnDragEnter;
            AssociatedObject.DragOver += AssociatedObjectOnDragOver;
            AssociatedObject.DragLeave += AssociatedObjectOnDragLeave;
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObjectOnLoaded;

            AdornerLayer.GetAdornerLayer(AssociatedObject).Add(_dropAdorner);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Drop -= OnDrop;
            AssociatedObject.DragEnter -= AssociatedObjectOnDragEnter;
            AssociatedObject.DragLeave -= AssociatedObjectOnDragLeave;
            AssociatedObject.DragOver -= AssociatedObjectOnDragOver;
        }

        private void AssociatedObjectOnDragEnter(object sender, DragEventArgs e)
        {
            _dropHereBanner.Show();
        }

        private void AssociatedObjectOnDragOver(object sender, DragEventArgs e)
        {
            
        }

        private void AssociatedObjectOnDragLeave(object sender, DragEventArgs e)
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
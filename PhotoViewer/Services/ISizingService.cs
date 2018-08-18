using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using PhotoViewer.Models;

namespace PhotoViewer.Services
{
    public interface ISizingService
    {
        ApplicationDpi CurrentDpi { get; }
    }

    internal class SizingService : ISizingService
    {
        public SizingService()
        {
            Matrix matrix;
            var source = PresentationSource.FromVisual(Application.Current.MainWindow);
            if (source != null)
            {
                matrix = source.CompositionTarget.TransformToDevice;
            }
            else
            {
                using (var hwndSource = new HwndSource(new HwndSourceParameters()))
                {
                    matrix = hwndSource.CompositionTarget.TransformToDevice;
                }
            }

            CurrentDpi = new ApplicationDpi(matrix.M11, matrix.M22);
        }

        public ApplicationDpi CurrentDpi { get; }
    }
}
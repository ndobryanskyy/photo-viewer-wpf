using System;
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
        private readonly Lazy<ApplicationDpi> _currentDpiLazy;

        public SizingService()
        {
            _currentDpiLazy = new Lazy<ApplicationDpi>(GetCurrentDpi);
        }

        private static ApplicationDpi GetCurrentDpi()
        {
            if (Application.Current.MainWindow != null)
            {
                var source = PresentationSource.FromVisual(Application.Current.MainWindow);
                if (source?.CompositionTarget != null)
                {
                    return MatrixToDpi(source.CompositionTarget.TransformToDevice);
                }
            }
            
            using (var hwndSource = new HwndSource(new HwndSourceParameters()))
            {
                var matrix = hwndSource.CompositionTarget?.TransformToDevice ?? Matrix.Identity;
                return MatrixToDpi(matrix);
            }
        }

        public ApplicationDpi CurrentDpi => _currentDpiLazy.Value;

        private static ApplicationDpi MatrixToDpi(Matrix matrix) => new ApplicationDpi(matrix.M11, matrix.M22);
    }
}
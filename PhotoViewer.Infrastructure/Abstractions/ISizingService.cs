using PhotoViewer.Infrastructure.Models;

namespace PhotoViewer.Infrastructure
{
    public interface ISizingService
    {
        ApplicationDpi CurrentDpi { get; }
    }
}
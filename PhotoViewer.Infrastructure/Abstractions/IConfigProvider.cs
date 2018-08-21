namespace PhotoViewer.Infrastructure
{
    public interface IConfigProvider
    {
        double ThumbnailSizeLimit { get; }

        string AppName { get; }
    }
}
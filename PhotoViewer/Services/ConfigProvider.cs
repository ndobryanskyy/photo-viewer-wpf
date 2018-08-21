using PhotoViewer.Infrastructure;
using PhotoViewer.Properties;

namespace PhotoViewer.Services
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly Settings _appSettings;

        public ConfigProvider()
        {
            _appSettings = Settings.Default;
        }

        public double ThumbnailSizeLimit => _appSettings.ThumbnailSizeLimiter;

        public string AppName => Resources.AppName;
    }
}
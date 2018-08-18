namespace PhotoViewer.Models
{
    public struct ApplicationDpi
    {
        public ApplicationDpi(double dpiX, double dpiY)
        {
            DpiX = dpiX;
            DpiY = dpiY;
        }

        public double DpiX { get; }

        public double DpiY { get; }
    }
}
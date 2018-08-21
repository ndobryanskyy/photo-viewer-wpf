using System.Windows.Controls;
using Prism.Regions;

namespace PhotoViewer.Views
{
    public partial class GalleryPage : UserControl, IRegionMemberLifetime
    {
        public GalleryPage()
        {
            InitializeComponent();
        }

        public bool KeepAlive { get; } = true;
    }
}

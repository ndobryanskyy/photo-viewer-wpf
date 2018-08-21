using System.Windows.Controls;
using Prism.Regions;

namespace PhotoViewer.Views
{
    public partial class CarouselPage : UserControl, IRegionMemberLifetime
    {
        public CarouselPage()
        {
            InitializeComponent();
            
        }

        public bool KeepAlive { get; } = false;
    }
}
using System.Collections.Generic;
using System.Linq;

namespace PhotoViewer.Models.EventArgs
{
    public class ImagesDroppedEventArgs : System.EventArgs
    {
        public ImagesDroppedEventArgs(IEnumerable<string> paths)
        {
            Paths = paths.ToArray();
        }

        public string[] Paths { get; }
    }
}
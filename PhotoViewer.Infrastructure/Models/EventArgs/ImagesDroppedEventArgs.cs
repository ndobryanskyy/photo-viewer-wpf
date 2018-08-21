using System.Collections.Generic;
using System.Linq;

namespace PhotoViewer.Infrastructure.Models.EventArgs
{
    public class ImagesDroppedEventArgs : System.EventArgs
    {
        public ImagesDroppedEventArgs(IEnumerable<string> filePaths)
        {
            FilePaths = filePaths.ToArray();
        }

        public string[] FilePaths { get; }
    }
}
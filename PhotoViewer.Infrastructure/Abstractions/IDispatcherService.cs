using System;
using System.Threading.Tasks;

namespace PhotoViewer.Infrastructure
{
    public interface IDispatcherService
    {
        Task ExecuteOnUIThreadAsync(Action action);
    }
}
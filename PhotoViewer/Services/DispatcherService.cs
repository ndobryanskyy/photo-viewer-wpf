using System;
using System.Threading.Tasks;
using System.Windows;
using PhotoViewer.Infrastructure;

namespace PhotoViewer.Services
{
    public class DispatcherService : IDispatcherService
    {
        public async Task ExecuteOnUIThreadAsync(Action action)
        {
            var dispatcher = Application.Current.Dispatcher 
                             ?? throw new InvalidOperationException("Called at unexpected time. Dispatcher could not be retrieved.");

            await dispatcher.InvokeAsync(action);
        }
    }
}
using Prism.Events;

namespace PhotoViewer.Infrastructure.Events
{
    public class ApplicationTitleChangedEvent : PubSubEvent<string>
    {
    }
}
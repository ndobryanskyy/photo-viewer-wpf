using System.Linq;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Input;

namespace PhotoViewer.Infrastructure.Behaviors
{
    /// <summary>
    /// This causes this System.Window.Data Error: Cannot find governing FrameworkElement or FrameworkContentElement for target element.
    /// But without it, due to not detaching <see cref="KeyTrigger"/> was causing memory leaks (View was not GC`ed) which lead  to undesired behavior.
    /// The problem with error popping up was not solved due to the lack of time.
    /// </summary>
    public class DetachKeyTriggersBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.Unloaded -= OnUnloaded;
            }
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var triggers = Interaction.GetTriggers(sender as FrameworkElement);

            if (triggers != null)
            {
                foreach (var trigger in triggers.OfType<KeyTrigger>().ToArray())
                {
                    trigger.Detach();
                    triggers.Remove(trigger);
                }
            }
        }
    }
}
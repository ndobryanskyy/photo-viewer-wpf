using System.Windows;
using System.Windows.Controls;

namespace PhotoViewer.Infrastructure.Controls
{
    [TemplatePart(Name = "RootGrid")]
    [TemplateVisualState(GroupName = "VisibilityStates", Name = "Hidden")]
    [TemplateVisualState(GroupName = "VisibilityStates", Name = "Shown")]
    public class DropHereBanner : Control
    {
        static DropHereBanner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropHereBanner), new FrameworkPropertyMetadata(typeof(DropHereBanner)));
        }

        public void Show()
        {
            VisualStateManager.GoToState(this, "Shown", true);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "Hidden", true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            VisualStateManager.GoToState(this, "Hidden", false);
        }
    }
}
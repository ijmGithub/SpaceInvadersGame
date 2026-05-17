using System.Windows;
using SpaceInvadersGame.Services;

namespace SpaceInvadersGame.Helpers
{
    public static class InputServiceBehavior
    {
        public static readonly DependencyProperty InputServiceProperty = DependencyProperty.RegisterAttached(
            "InputService",
            typeof(IInputService),
            typeof(InputServiceBehavior),
            new PropertyMetadata(null, OnInputServiceChanged));

        public static IInputService? GetInputService(DependencyObject obj)
        {
            return (IInputService?)obj.GetValue(InputServiceProperty);
        }

        public static void SetInputService(DependencyObject obj, IInputService? value)
        {
            obj.SetValue(InputServiceProperty, value);
        }

        private static void OnInputServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement element)
            {
                return;
            }

            if (e.OldValue is IInputService oldService)
            {
                oldService.Detach(element);
            }

            if (e.NewValue is IInputService newService)
            {
                newService.Attach(element);
            }
        }
    }
}

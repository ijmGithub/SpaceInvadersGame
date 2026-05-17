using System;
using System.Windows;

namespace SpaceInvadersGame.Services
{
    public interface IInputService
    {
        event EventHandler<InputActionEventArgs>? ActionChanged;

        bool IsLeftPressed { get; }

        bool IsRightPressed { get; }

        bool IsShootPressed { get; }

        void Attach(UIElement element);

        void Detach(UIElement element);

        void Reset();
    }
}

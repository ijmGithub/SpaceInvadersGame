using System;

namespace SpaceInvadersGame.Services
{
    public sealed class InputActionEventArgs : EventArgs
    {
        public InputActionEventArgs(InputAction action, bool isPressed)
        {
            Action = action;
            IsPressed = isPressed;
        }

        public InputAction Action { get; }

        public bool IsPressed { get; }
    }
}

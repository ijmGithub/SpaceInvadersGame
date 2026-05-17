using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace SpaceInvadersGame.Services
{
    public class InputService : IInputService
    {
        private readonly HashSet<Key> _pressedKeys = new();
        private UIElement? _attachedElement;

        public event EventHandler<InputActionEventArgs>? ActionChanged;

        public bool IsLeftPressed => _pressedKeys.Contains(Key.Left) || _pressedKeys.Contains(Key.A);

        public bool IsRightPressed => _pressedKeys.Contains(Key.Right) || _pressedKeys.Contains(Key.D);

        public bool IsShootPressed => _pressedKeys.Contains(Key.Space) || _pressedKeys.Contains(Key.Enter);

        public void Attach(UIElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (_attachedElement == element)
            {
                return;
            }

            Detach(_attachedElement);

            _attachedElement = element;
            _attachedElement.PreviewKeyDown += OnPreviewKeyDown;
            _attachedElement.PreviewKeyUp += OnPreviewKeyUp;
            _attachedElement.LostKeyboardFocus += OnLostKeyboardFocus;
        }

        public void Detach(UIElement element)
        {
            if (element is null)
            {
                return;
            }

            element.PreviewKeyDown -= OnPreviewKeyDown;
            element.PreviewKeyUp -= OnPreviewKeyUp;
            element.LostKeyboardFocus -= OnLostKeyboardFocus;

            if (_attachedElement == element)
            {
                _attachedElement = null;
            }
        }

        public void Reset()
        {
            var previousLeft = IsLeftPressed;
            var previousRight = IsRightPressed;
            var previousShoot = IsShootPressed;

            _pressedKeys.Clear();

            RaiseActionChanged(InputAction.MoveLeft, previousLeft, false);
            RaiseActionChanged(InputAction.MoveRight, previousRight, false);
            RaiseActionChanged(InputAction.Shoot, previousShoot, false);
        }

        private void OnPreviewKeyDown(object? sender, KeyEventArgs e)
        {
            if (ProcessKey(e.Key, true))
            {
                e.Handled = true;
            }
        }

        private void OnPreviewKeyUp(object? sender, KeyEventArgs e)
        {
            if (ProcessKey(e.Key, false))
            {
                e.Handled = true;
            }
        }

        private void OnLostKeyboardFocus(object? sender, KeyboardFocusChangedEventArgs e)
        {
            Reset();
        }

        private bool ProcessKey(Key key, bool isPressed)
        {
            var action = MapKeyToAction(key);
            if (action is null)
            {
                return false;
            }

            if (isPressed)
            {
                if (!_pressedKeys.Add(key))
                {
                    return false;
                }
            }
            else
            {
                if (!_pressedKeys.Remove(key))
                {
                    return false;
                }
            }

            RaiseActionChanged(action.Value, isPressed);
            return true;
        }

        private static InputAction? MapKeyToAction(Key key)
        {
            return key switch
            {
                Key.Left or Key.A => InputAction.MoveLeft,
                Key.Right or Key.D => InputAction.MoveRight,
                Key.Space or Key.Enter => InputAction.Shoot,
                _ => null,
            };
        }

        private void RaiseActionChanged(InputAction action, bool isPressed)
        {
            ActionChanged?.Invoke(this, new InputActionEventArgs(action, isPressed));
        }

        private void RaiseActionChanged(InputAction action, bool previousState, bool currentState)
        {
            if (previousState != currentState)
            {
                RaiseActionChanged(action, currentState);
            }
        }
    }
}

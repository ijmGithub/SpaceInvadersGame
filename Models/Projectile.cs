using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SpaceInvadersGame.Models
{
    public class Projectile : INotifyPropertyChanged, ICollidable
    {
        private static int _nextId = 1;
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private double _speed;
        private double _directionX;
        private double _directionY;
        private bool _isActive;

        public Projectile(double x, double y, double width = 8, double height = 16, double speed = 400, double directionX = 0, double directionY = -1, bool isActive = true)
        {
            Id = _nextId++;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Speed = speed;
            DirectionX = directionX;
            DirectionY = directionY;
            IsActive = isActive;
        }

        public int Id { get; }

        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public double Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
        }

        public double DirectionX
        {
            get => _directionX;
            set => SetProperty(ref _directionX, value);
        }

        public double DirectionY
        {
            get => _directionY;
            set => SetProperty(ref _directionY, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public Rect Bounds => new(X, Y, Width, Height);

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Update(double elapsedSeconds)
        {
            if (!IsActive)
            {
                return;
            }

            X += DirectionX * Speed * elapsedSeconds;
            Y += DirectionY * Speed * elapsedSeconds;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

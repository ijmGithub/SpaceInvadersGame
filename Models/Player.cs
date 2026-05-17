using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SpaceInvadersGame.Models
{
    public class Player : INotifyPropertyChanged, ICollidable
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private double _speed;
        private int _lives;
        private int _score;

        public Player(double x = 0, double y = 0, double width = 48, double height = 48, double speed = 220, int lives = 3, int score = 0)
        {
            Id = 0; // Player always has Id 0
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Speed = speed;
            Lives = lives;
            Score = score;
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

        public int Lives
        {
            get => _lives;
            set => SetProperty(ref _lives, value);
        }

        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        public Rect Bounds => new(X, Y, Width, Height);

        public bool IsActive => Lives > 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Move(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }

        public void AddScore(int points)
        {
            if (points <= 0)
            {
                return;
            }

            Score += points;
        }

        public void LoseLife()
        {
            if (Lives <= 0)
            {
                return;
            }

            Lives -= 1;
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

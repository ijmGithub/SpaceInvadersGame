using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SpaceInvadersGame.Models
{
    public class Enemy : INotifyPropertyChanged, ICollidable
    {
        private double _x;
        private double _y;
        private int _health;
        private bool _isAlive;
        private double _speed;
        private double _width;
        private double _height;
        private string _type;

        public Enemy(int id, double x, double y, int health = 1, double speed = 120, string type = "Basic", double width = 32, double height = 32)
        {
            Id = id;
            X = x;
            Y = y;
            Health = health;
            Speed = speed;
            Type = type;
            Width = width;
            Height = height;
            IsAlive = health > 0;
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

        public int Health
        {
            get => _health;
            set
            {
                if (SetProperty(ref _health, value))
                {
                    IsAlive = _health > 0;
                }
            }
        }

        public bool IsAlive
        {
            get => _isAlive;
            private set => SetProperty(ref _isAlive, value);
        }

        public double Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
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

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public Rect Bounds => new(X, Y, Width, Height);

        public bool IsActive => IsAlive;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Move(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || !IsAlive)
            {
                return;
            }

            Health = Math.Max(0, Health - amount);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
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

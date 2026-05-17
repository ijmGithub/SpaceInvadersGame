using System;
using System.Windows.Threading;

namespace SpaceInvadersGame.Services
{
    public class GameLoopService : IGameLoopService
    {
        private readonly DispatcherTimer _timer;

        public event EventHandler? Tick;

        public GameLoopService(TimeSpan? interval = null)
        {
            _timer = new DispatcherTimer(DispatcherPriority.Render, Dispatcher.CurrentDispatcher)
            {
                Interval = interval ?? TimeSpan.FromMilliseconds(16)
            };

            _timer.Tick += OnTimerTick;
        }

        public TimeSpan Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        public bool IsRunning => _timer.IsEnabled;

        public void Start()
        {
            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        public void Reset()
        {
            Stop();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}

using System;

namespace SpaceInvadersGame.Services
{
    public interface IGameLoopService
    {
        event EventHandler? Tick;

        TimeSpan Interval { get; set; }

        bool IsRunning { get; }

        void Start();

        void Stop();

        void Reset();
    }
}

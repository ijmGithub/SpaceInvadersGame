using System;
using System.Windows;
using System.Windows.Input;
using SpaceInvadersGame.Commands;

namespace SpaceInvadersGame.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public event EventHandler? PlayRequested;
        public event EventHandler? ScoresRequested;

        public MenuViewModel()
        {
            PlayCommand = new RelayCommand(_ => Play());
            ScoresCommand = new RelayCommand(_ => ShowScores());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        public string Title { get; } = "Space Invaders";

        public string Subtitle { get; } = "Presiona Start para comenzar la batalla espacial.";

        public ICommand PlayCommand { get; }

        public ICommand ScoresCommand { get; }

        public ICommand ExitCommand { get; }

        private void Play()
        {
            PlayRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ShowScores()
        {
            ScoresRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}

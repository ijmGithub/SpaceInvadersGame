using System;
using System.Windows.Input;
using SpaceInvadersGame.Commands;
using SpaceInvadersGame.Models;
using SpaceInvadersGame.Services;

namespace SpaceInvadersGame.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ScoreViewModel _scoreViewModel;
        private object _currentViewModel;

        public MainViewModel()
        {
            var scoreService = new ScoreService();
            _scoreViewModel = new ScoreViewModel(scoreService);
            var audioService = new AudioService();
            var menuViewModel = new MenuViewModel();
            menuViewModel.PlayRequested += (_, _) => NavigateTo(ViewType.Game);
            menuViewModel.ScoresRequested += (_, _) => NavigateTo(ViewType.Score);
            _navigationService = new NavigationService(
                menuViewModel,
                new GameViewModel(new GameLoopService(), new CollisionService(), new InputService(), scoreService, audioService),
                _scoreViewModel);

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            CurrentViewModel = _navigationService.CurrentViewModel!;
            audioService.PlayBackgroundMusic();

            NavigateMenuCommand = new RelayCommand(_ => NavigateTo(ViewType.Menu));
            NavigateGameCommand = new RelayCommand(_ => NavigateTo(ViewType.Game));
            NavigateScoreCommand = new RelayCommand(_ => NavigateTo(ViewType.Score));
        }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand NavigateMenuCommand { get; }

        public ICommand NavigateGameCommand { get; }

        public ICommand NavigateScoreCommand { get; }

        private void OnCurrentViewModelChanged(object? sender, EventArgs e)
        {
            if (_navigationService.CurrentViewType == ViewType.Score)
            {
                _scoreViewModel.Refresh();
            }

            CurrentViewModel = _navigationService.CurrentViewModel!;
        }

        private void NavigateTo(ViewType viewType)
        {
            _navigationService.NavigateTo(viewType);
        }
    }
}

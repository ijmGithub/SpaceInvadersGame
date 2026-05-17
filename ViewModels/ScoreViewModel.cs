using SpaceInvadersGame.Services;

namespace SpaceInvadersGame.ViewModels
{
    public class ScoreViewModel : BaseViewModel
    {
        private readonly IScoreService _scoreService;

        public ScoreViewModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
            CurrentScore = _scoreService.CurrentScore;
            HighScore = _scoreService.HighScore;
        }

        public int CurrentScore { get; private set; }

        public int HighScore { get; private set; }

        public void Refresh()
        {
            _scoreService.LoadHighScore();
            CurrentScore = _scoreService.CurrentScore;
            HighScore = _scoreService.HighScore;
            OnPropertyChanged(nameof(CurrentScore));
            OnPropertyChanged(nameof(HighScore));
        }
    }
}

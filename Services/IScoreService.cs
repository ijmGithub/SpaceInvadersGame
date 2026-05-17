namespace SpaceInvadersGame.Services
{
    public interface IScoreService
    {
        int CurrentScore { get; }

        int HighScore { get; }

        void AddPoints(int points);

        void ResetScore();

        void LoadHighScore();

        void SaveHighScore();
    }
}

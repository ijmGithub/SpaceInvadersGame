using System;
using System.IO;

namespace SpaceInvadersGame.Services
{
    public class ScoreService : IScoreService
    {
        private const string ScoreFileName = "highscore.txt";
        private readonly string _storageFilePath;

        public ScoreService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var gameFolder = Path.Combine(appData, "SpaceInvadersGame");
            Directory.CreateDirectory(gameFolder);
            _storageFilePath = Path.Combine(gameFolder, ScoreFileName);

            LoadHighScore();
        }

        public int CurrentScore { get; private set; }

        public int HighScore { get; private set; }

        public void AddPoints(int points)
        {
            if (points <= 0)
            {
                return;
            }

            CurrentScore += points;

            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
            }
        }

        public void ResetScore()
        {
            CurrentScore = 0;
        }

        public void LoadHighScore()
        {
            if (!File.Exists(_storageFilePath))
            {
                HighScore = 0;
                return;
            }

            try
            {
                var content = File.ReadAllText(_storageFilePath).Trim();
                if (int.TryParse(content, out var savedScore) && savedScore >= 0)
                {
                    HighScore = savedScore;
                }
                else
                {
                    HighScore = 0;
                }
            }
            catch
            {
                HighScore = 0;
            }
        }

        public void SaveHighScore()
        {
            try
            {
                var directory = Path.GetDirectoryName(_storageFilePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_storageFilePath, HighScore.ToString());
            }
            catch
            {
                // Ignore save failures to avoid crashing the game.
            }
        }
    }
}

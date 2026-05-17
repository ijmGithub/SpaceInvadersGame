using System.Collections.ObjectModel;
using SpaceInvadersGame.Models;

namespace SpaceInvadersGame.Services
{
    public interface IEnemySpawnerService
    {
        ObservableCollection<Enemy> SpawnWave(
            int rows,
            int columns,
            double startX,
            double startY,
            double spacingX,
            double spacingY,
            double enemyWidth = 40,
            double enemyHeight = 32,
            double speed = 60,
            int health = 1,
            string enemyType = "Invader");
    }
}

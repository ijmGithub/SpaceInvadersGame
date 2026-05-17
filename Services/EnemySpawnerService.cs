using System.Collections.ObjectModel;
using SpaceInvadersGame.Models;

namespace SpaceInvadersGame.Services
{
    public class EnemySpawnerService : IEnemySpawnerService
    {
        public ObservableCollection<Enemy> SpawnWave(
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
            string enemyType = "Invader")
        {
            var enemies = new ObservableCollection<Enemy>();
            var id = 1;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < columns; col++)
                {
                    var x = startX + col * spacingX;
                    var y = startY + row * spacingY;

                    enemies.Add(new Enemy(
                        id: id++, 
                        x: x,
                        y: y,
                        health: health,
                        speed: speed,
                        type: enemyType,
                        width: enemyWidth,
                        height: enemyHeight));
                }
            }

            return enemies;
        }
    }
}

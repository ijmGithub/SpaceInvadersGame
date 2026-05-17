using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SpaceInvadersGame.Commands;
using SpaceInvadersGame.Models;
using SpaceInvadersGame.Services;

namespace SpaceInvadersGame.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private readonly IGameLoopService _gameLoopService;
        private readonly ICollisionService _collisionService;
        private readonly IInputService _inputService;
        private readonly IScoreService _scoreService;
        private readonly IAudioService _audioService;
        private bool _isGameRunning;
        private string _statusMessage = string.Empty;
        private Player _player = new Player();

        public GameViewModel()
            : this(new GameLoopService(), new CollisionService(), new InputService(), new ScoreService())
        {
        }

        public GameViewModel(IGameLoopService gameLoopService, ICollisionService collisionService, IInputService inputService, IScoreService scoreService, IAudioService? audioService = null)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _collisionService = collisionService ?? throw new ArgumentNullException(nameof(collisionService));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            _audioService = audioService ?? new AudioService();

            Player = new Player(x: 360, y: 520);
            Enemies = new ObservableCollection<Enemy>();
            Projectiles = new ObservableCollection<Projectile>();

            StartGameCommand = new RelayCommand(_ => StartGame(), _ => !IsGameRunning);
            PauseGameCommand = new RelayCommand(_ => PauseGame(), _ => IsGameRunning);

            _gameLoopService.Tick += OnGameTick;
            _inputService.ActionChanged += OnInputActionChanged;
            StatusMessage = "Listo";
        }

        public Player Player
        {
            get => _player;
            private set => SetProperty(ref _player, value);
        }

        public ObservableCollection<Enemy> Enemies { get; }

        public ObservableCollection<Projectile> Projectiles { get; }

        public ICommand StartGameCommand { get; }

        public ICommand PauseGameCommand { get; }

        public bool IsGameRunning
        {
            get => _isGameRunning;
            private set
            {
                if (SetProperty(ref _isGameRunning, value))
                {
                    OnPropertyChanged(nameof(CanShoot));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public IInputService InputService => _inputService;

        public string StatusMessage
        {
            get => _statusMessage;
            private set
            {
                if (SetProperty(ref _statusMessage, value))
                {
                    OnPropertyChanged(nameof(ShowEndGameMessage));
                }
            }
        }

        public bool ShowEndGameMessage => !IsGameRunning && !string.IsNullOrEmpty(StatusMessage) && StatusMessage != "Listo" && StatusMessage != "Pausado";

        public bool CanShoot => IsGameRunning && Player.IsActive;

        private void StartGame()
        {
            ResetGame();
            _gameLoopService.Start();
            _audioService.PlayBackgroundMusic();
            IsGameRunning = true;
            StatusMessage = "Jugando";
        }

        private void PauseGame()
        {
            _gameLoopService.Stop();
            _audioService.PauseBackgroundMusic();
            IsGameRunning = false;
            StatusMessage = "Pausado";
        }

        private void Shoot()
        {
            if (!Player.IsActive)
            {
                return;
            }

            var projectile = new Projectile(
                x: Player.X + Player.Width / 2 - 4,
                y: Player.Y - 16,
                width: 8,
                height: 16,
                speed: 520,
                directionX: 0,
                directionY: -1);

            Projectiles.Add(projectile);
        }

        private void OnGameTick(object? sender, EventArgs e)
        {
            var deltaSeconds = _gameLoopService.Interval.TotalSeconds;
            UpdatePlayerMovement(deltaSeconds);
            UpdateProjectiles(deltaSeconds);
            DetectCollisions();
            CleanupInactiveEntities();
            UpdateGameState();
        }

        private void UpdatePlayerMovement(double elapsedSeconds)
        {
            if (!Player.IsActive || !IsGameRunning)
            {
                return;
            }

            double deltaX = 0;
            if (_inputService.IsLeftPressed)
            {
                deltaX -= Player.Speed * elapsedSeconds;
            }
            if (_inputService.IsRightPressed)
            {
                deltaX += Player.Speed * elapsedSeconds;
            }

            // Clamp to screen bounds (assuming 800 width)
            var newX = Player.X + deltaX;
            newX = Math.Max(0, Math.Min(800 - Player.Width, newX));
            Player.X = newX;
        }

        private void UpdateProjectiles(double elapsedSeconds)
        {
            foreach (var projectile in Projectiles.Where(p => p.IsActive).ToList())
            {
                projectile.Update(elapsedSeconds);

                if (projectile.Y < -projectile.Height || projectile.Y > 720)
                {
                    projectile.Deactivate();
                }
            }
        }

        private void DetectCollisions()
        {
            var collisions = _collisionService.DetectCollisions(Projectiles.Cast<ICollidable>(), Enemies.Cast<ICollidable>());

            foreach (var hit in collisions)
            {
                if (hit.Source is Projectile projectile && hit.Target is Enemy enemy)
                {
                    projectile.Deactivate();
                    enemy.TakeDamage(1);
                    AddScore(10);
                }
                else if (hit.Target is Projectile projectile2 && hit.Source is Enemy enemy2)
                {
                    projectile2.Deactivate();
                    enemy2.TakeDamage(1);
                    AddScore(10);
                }
            }
        }

        private void CleanupInactiveEntities()
        {
            for (var i = Projectiles.Count - 1; i >= 0; i--)
            {
                if (!Projectiles[i].IsActive)
                {
                    Projectiles.RemoveAt(i);
                }
            }

            for (var i = Enemies.Count - 1; i >= 0; i--)
            {
                if (!Enemies[i].IsAlive)
                {
                    Enemies.RemoveAt(i);
                }
            }
        }

        private void AddScore(int points)
        {
            Player.AddScore(points);
            _scoreService.AddPoints(points);
        }

        private void OnInputActionChanged(object? sender, InputActionEventArgs e)
        {
            if (e.Action == InputAction.Shoot && e.IsPressed && IsGameRunning && Player.IsActive)
            {
                Shoot();
            }
        }

        private void UpdateGameState()
        {
            if (!Player.IsActive)
            {
                PauseGame();
                StatusMessage = "¡Juego terminado! Inténtalo de nuevo.";
                _scoreService.SaveHighScore();
                return;
            }

            if (!Enemies.Any())
            {
                PauseGame();
                StatusMessage = "¡Enhorabuena! Has ganado.";
                _scoreService.SaveHighScore();
            }
        }

        private void ResetGame()
        {
            Projectiles.Clear();
            Enemies.Clear();
            _scoreService.ResetScore();
            Player = new Player(x: 360, y: 520);
            CreateEnemies();
        }

        private void CreateEnemies()
        {
            const int rows = 3;
            const int cols = 8;
            const double spacingX = 60;
            const double spacingY = 52;
            const double startX = 120;
            const double startY = 80;

            var id = 1;
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var enemy = new Enemy(
                        id: id++, 
                        x: startX + col * spacingX,
                        y: startY + row * spacingY,
                        health: 1,
                        speed: 60,
                        type: "Invader",
                        width: 40,
                        height: 32);

                    Enemies.Add(enemy);
                }
            }
        }
    }
}

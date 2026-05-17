using System;
using System.IO;
using System.Windows.Media;

namespace SpaceInvadersGame.Services
{
    public class AudioService : IAudioService
    {
        private readonly MediaPlayer _musicPlayer;

        public AudioService()
        {
            _musicPlayer = new MediaPlayer();
            _musicPlayer.MediaEnded += (sender, args) =>
            {
                _musicPlayer.Position = TimeSpan.Zero;
                _musicPlayer.Play();
            };
            _musicPlayer.Volume = 0.35;
            OpenMusicFile();
        }

        private void OpenMusicFile()
        {
            var soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "SpaceInvadersTheme.wav");
            if (File.Exists(soundPath))
            {
                _musicPlayer.Open(new Uri(soundPath, UriKind.Absolute));
            }
        }

        public void PlayBackgroundMusic()
        {
            _musicPlayer.Position = TimeSpan.Zero;
            _musicPlayer.Play();
        }

        public void PauseBackgroundMusic()
        {
            _musicPlayer.Pause();
        }

        public void StopBackgroundMusic()
        {
            _musicPlayer.Stop();
        }
    }
}

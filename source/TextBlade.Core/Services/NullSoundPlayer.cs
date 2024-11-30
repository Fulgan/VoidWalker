using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBlade.Core.Services
{
    public sealed class NullSoundPlayer : ISoundPlayer
    {
        public void Dispose()
        {
        }

        public event AsyncCompletedEventHandler? LoadCompleted;
        public bool IsLoadCompleted => true;
        public string SoundLocation { get; set; } = string.Empty;
        public void Load()
        {
            OnLoadCompleted(new AsyncCompletedEventArgs(null, false, null));
        }


        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            OnLoadCompleted(new AsyncCompletedEventArgs(null, false, null));
            return Task.CompletedTask;
        }


        public void Play()
        {
        }

        public void PlayLooping()
        {
        }

        public void PlaySync()
        {
        }

        public void Stop()
        {
        }

        private void OnLoadCompleted(AsyncCompletedEventArgs e)
        {
            LoadCompleted?.Invoke(this, e);
        }
    }
}

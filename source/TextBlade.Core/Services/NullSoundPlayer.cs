
namespace TextBlade.Core.Services
{
    public sealed class NullSoundPlayer : ISoundPlayer
    {
        public event Action? OnPlaybackDone;

        public void Dispose()
        {
        }

        public void Load(string audioFile)
        {
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }
    }
}


namespace TextBlade.Core.Services
{
    public sealed class NullSoundPlayer : ISoundPlayer
    {
        public Action? OnPlaybackComplete { get; set; }
        public bool LoopPlayback { get; set; }

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

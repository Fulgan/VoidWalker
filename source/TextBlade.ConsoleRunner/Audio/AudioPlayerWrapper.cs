using SonicBoom;
using TextBlade.Core.Audio;

namespace TextBlade.ConsoleRunner.Audio;

public class AudioPlayerWrapper : AudioPlayer, ISoundPlayer
{
    public AudioPlayerWrapper(bool loopPlayback = false)
    {
        base.LoopPlayback = loopPlayback;    
    }

    public void Play(string audio)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(audio);

        base.Load(audio);
        base.Play();
    }
}

using SonicBoom;
using TextBlade.Core.Audio;

namespace TextBlade.ConsoleRunner.Audio;

public class AudioPlayerWrapper : AudioPlayer, ISoundPlayer
{
    public void Play(string audio)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(audio);

        base.Load(audio);
        base.Play();
    }
}

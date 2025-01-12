namespace TextBlade.Core.Audio;

public interface ISoundPlayer
{
    public void Play(string audio, string channel = "stereo");
    public void Stop();
}

namespace TextBlade.Core.Services;

public interface ISoundPlayer : IDisposable
{
    public event Action? OnPlaybackDone;
    void Load(string audioFile);
    void Play();
    void Stop();
}
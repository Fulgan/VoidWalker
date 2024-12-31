namespace TextBlade.Core.Services;

public interface ISoundPlayer : IDisposable
{
    public event Action? OnPlaybackComplete;
    void Load(string audioFile);
    void Play();
    void Stop();
}
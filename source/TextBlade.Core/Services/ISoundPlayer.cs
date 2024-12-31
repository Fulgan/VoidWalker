namespace TextBlade.Core.Services;

public interface ISoundPlayer : IDisposable
{
    public Action? OnPlaybackComplete { get; set; }
    public bool LoopPlayback { get; set; }
    void Load(string audioFile);
    void Play();
    void Stop();
}
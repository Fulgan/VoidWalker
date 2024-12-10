namespace TextBlade.Core.Services;

public interface ISoundPlayer : IDisposable
{
    void Load(string audioFile);
    void Play();
    void Stop();
}
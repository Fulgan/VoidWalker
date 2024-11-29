using System.ComponentModel;

namespace TextBlade.Core.Services;

public interface ISoundPlayer : IDisposable
{
    public event AsyncCompletedEventHandler? LoadCompleted;
    bool IsLoadCompleted { get; }
    string SoundLocation { get; set; }
    void Load();
    Task LoadAsync(CancellationToken cancellationToken = default);
    void Play();
    void PlayLooping();
    void PlaySync();
    void Stop();
}
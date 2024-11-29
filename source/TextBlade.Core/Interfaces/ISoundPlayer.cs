using System.ComponentModel;

namespace TextBlade.Core.Interfaces;

public interface ISoundPlayer: IDisposable
{
    public event AsyncCompletedEventHandler? LoadCompleted;
    bool IsLoadCompleted { get; }
    string SoundLocation { get; set; }
    void Load();
    void LoadAsync();
    void Play();
    void PlayLooping();
    void PlaySync();
    void Stop();
}
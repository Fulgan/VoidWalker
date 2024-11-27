using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Runtime.Serialization;
using TextBlade.Core.Interfaces;

namespace TextBlade.Plateform.Windows;

public class WindowsSoundPlayer : ISoundPlayer, IDisposable
{
    private readonly SoundPlayer _soundPlayer = new();

    [Obsolete("Obsolete")]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ((ISerializable)_soundPlayer).GetObjectData(info, context);
    }

    public void Load()
    {
        _soundPlayer.Load();
    }

    public void LoadAsync()
    {
        _soundPlayer.LoadAsync();
    }

    public void Play()
    {
        _soundPlayer.Play();
    }

    public void PlayLooping()
    {
        _soundPlayer.PlayLooping();
    }

    public void PlaySync()
    {
        _soundPlayer.PlaySync();
    }

    public void Stop()
    {
        _soundPlayer.Stop();
    }

    public bool IsLoadCompleted => _soundPlayer.IsLoadCompleted;

    public int LoadTimeout
    {
        get => _soundPlayer.LoadTimeout;
        set => _soundPlayer.LoadTimeout = value;
    }

    public string SoundLocation
    {
        get => _soundPlayer.SoundLocation;
        set => _soundPlayer.SoundLocation = value;
    }

    public Stream? Stream
    {
        get => _soundPlayer.Stream;
        set => _soundPlayer.Stream = value;
    }

    public object? Tag
    {
        get => _soundPlayer.Tag;
        set => _soundPlayer.Tag = value;
    }

    public event AsyncCompletedEventHandler? LoadCompleted
    {
        add => _soundPlayer.LoadCompleted += value;
        remove => _soundPlayer.LoadCompleted -= value;
    }

    public event EventHandler? SoundLocationChanged
    {
        add => _soundPlayer.SoundLocationChanged += value;
        remove => _soundPlayer.SoundLocationChanged -= value;
    }

    public event EventHandler? StreamChanged
    {
        add => _soundPlayer.StreamChanged += value;
        remove => _soundPlayer.StreamChanged -= value;
    }

    public void Dispose()
    {
        _soundPlayer.Dispose();
    }
}
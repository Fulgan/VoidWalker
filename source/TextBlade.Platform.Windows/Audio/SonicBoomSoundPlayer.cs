using SonicBoom;
using TextBlade.Core.Services;

namespace TextBlade.Platform.Windows.Audio;

public class SonicBoomSoundPlayer : ISoundPlayer
{
    public Action? OnPlaybackComplete
    {
        get => _onPlaybackComplete;
        set
        {
            _onPlaybackComplete = value;
            if (_audioPlayer != null)
            {
                _audioPlayer.OnPlaybackComplete += value;
            }
        }
    }
    
    public bool LoopPlayback
    {
        get => _loopPlayback;
        set
        {
            _loopPlayback = value;
            if (_audioPlayer != null)
            {
                _audioPlayer.LoopPlayback = value;
            }
        }
    }

    private const string SupportedAudioExtension = "ogg";
    private const float VolumeMultiplier = 0.5f;

    private AudioPlayer? _audioPlayer;
    private bool _loopPlayback;
    private Action? _onPlaybackComplete;
    private bool _disposedValue;

    public void Load(string audioFile)
    {
        _audioPlayer = new AudioPlayer();
        _audioPlayer.Load($"{audioFile}.{SupportedAudioExtension}");
        _audioPlayer.Volume = VolumeMultiplier;
        _audioPlayer.LoopPlayback = LoopPlayback;
        _audioPlayer.OnPlaybackComplete += OnPlaybackComplete;
    }

    public void Play() => _audioPlayer?.Play();
    public void Stop() =>_audioPlayer?.Stop();
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _audioPlayer?.Dispose();
            }

            _audioPlayer = null;
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
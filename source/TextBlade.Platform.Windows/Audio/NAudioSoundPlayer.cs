using NAudio.Vorbis;
using NAudio.Wave;
using TextBlade.Core.Services;

namespace TextBlade.Platform.Windows.Audio;

public class NAudioSoundPlayer : ISoundPlayer
{
    public event Action? OnPlaybackComplete;

    private const string SupportedAudioExtension = "ogg";
    private const float VolumeMultiplier = 0.5f;

    private WaveOutEvent? _waveOut;
    private bool _disposedValue;

    public void Load(string audioFile)
    {
        var reader = new VorbisWaveReader($"{audioFile}.{SupportedAudioExtension}");
        _waveOut = new WaveOutEvent();
        _waveOut.Init(reader);
        _waveOut.Volume = VolumeMultiplier;

        _waveOut.PlaybackStopped += (sender, stoppedArgs) =>
        {
            if (!ShouldLoopPlayback)
            {
                OnPlaybackComplete?.Invoke();
                return;
            }

            // Loop playback
            reader.Seek(0, SeekOrigin.Begin);
            _waveOut.Init(reader);
            _waveOut?.Play();
        };
    }

    public void Play() => _waveOut?.Play();
    public void Stop() =>_waveOut?.Stop();
    
    private bool ShouldLoopPlayback => OnPlaybackComplete == null;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _waveOut?.Dispose();
            }

            _waveOut = null;
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
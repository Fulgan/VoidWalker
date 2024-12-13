using System.Runtime.Versioning;
using NAudio.Vorbis;
using NAudio.Wave;
using TextBlade.Core.Services;

namespace TextBlade.Platform.Windows.Audio;

[SupportedOSPlatform("windows")]
public class NAudioSoundPlayer : ISoundPlayer
{
    internal const string SupportedAudioExtension = "ogg";
    private const float VolumeMultiplier = 0.7f;

    private WaveOutEvent? _waveOut;
    private VorbisWaveReader? _reader;
    
    public void Load(string audioFile)
    {
        _reader = new VorbisWaveReader($"{audioFile}.{SupportedAudioExtension}");

        _waveOut = new WaveOutEvent();
        _waveOut.Init(_reader);
        _waveOut.Volume = VolumeMultiplier;

        // Don't do this for one-off audio. Which we don't have yet.
        _waveOut.PlaybackStopped += RewindAndPlay;
    }

    public void Play() => _waveOut?.Play();

    // Stop doesn't work ... 
    // NAudio is no longer maintained, gotta switch out.
    // There's no decent alternative, though ...
    public void Stop() => _waveOut?.Pause();

    private void RewindAndPlay(object? sender, StoppedEventArgs args)
    {
        _reader.Seek(0, SeekOrigin.Begin);
        Play();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _reader.Dispose();
            _waveOut?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

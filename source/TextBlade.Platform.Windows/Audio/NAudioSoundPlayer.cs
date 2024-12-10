using System.Runtime.Versioning;
using NAudio.Vorbis;
using NAudio.Wave;
using TextBlade.Core.Services;

namespace TextBlade.Platform.Windows.Audio;

[SupportedOSPlatform("windows")]
public class NAudioSoundPlayer : ISoundPlayer
{
    internal const string SupportedAudioExtension = "ogg";

    private WaveOutEvent? _waveOut;
    private VorbisWaveReader? _reader;
    
    public void Load(string audioFile)
    {
        
        _reader = new VorbisWaveReader($"{audioFile}.{SupportedAudioExtension}");
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_reader);
    }

    public void Play() => _waveOut?.Play();

    public void Stop() => _waveOut?.Stop();

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

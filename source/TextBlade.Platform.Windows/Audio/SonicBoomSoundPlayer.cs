using SonicBoom;
using System.Runtime.Versioning;
using TextBlade.Core.Services;

namespace TextBlade.Platform.Windows.Audio;

[SupportedOSPlatform("windows")]
public class SonicBoomSoundPlayer : ISoundPlayer
{
    internal const string SupportedAudioExtension = "ogg";
    private const float VolumeMultiplier = 0.5f;

    private AudioPlayer? _audioPlayer;
    
    public void Load(string audioFile)
    {
        _audioPlayer = new AudioPlayer($"{audioFile}.{SupportedAudioExtension}");
        _audioPlayer.LoopPlayback = true;
        _audioPlayer.Volume = VolumeMultiplier;
    }

    public void Play() => _audioPlayer?.Play();
    public void Stop() => _audioPlayer?.Stop();

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _audioPlayer?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

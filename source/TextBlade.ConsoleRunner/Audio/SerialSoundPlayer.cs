using SonicBoom;

namespace TextBlade.ConsoleRunner.Audio;

public class SerialSoundPlayer : IDisposable
{
    private readonly AudioPlayer _audioPlayer = new();
    private int _currentAudioId = 0;
    private IList<string>? _audiosToPlay;
    private bool _disposedValue;

    public SerialSoundPlayer()
    {
        _audioPlayer.OnPlaybackComplete += PlayNext;
    }

    public void Play(params string[] audios)
    {
        _audiosToPlay = audios;
        _currentAudioId = -1;
        PlayNext();
    }

    private void PlayNext()
    {
        _currentAudioId++;
        if (_currentAudioId >= _audiosToPlay.Count)
        {
            // We're done!
            _currentAudioId = 0;
            _audiosToPlay = Array.Empty<string>();
            _audioPlayer.Dispose();
            return;
        }

        _audioPlayer.Stop();
        _audioPlayer.Load(_audiosToPlay[_currentAudioId]);
        _audioPlayer.Play();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _audioPlayer.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

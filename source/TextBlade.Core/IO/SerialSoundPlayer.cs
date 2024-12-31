using TextBlade.Core.Services;

namespace TextBlade.Core.IO;

public class SerialSoundPlayer : IDisposable
{
    private readonly ISoundPlayer _soundPlayer;
    private int _currentAudioId = 0;
    private IList<string>? _audiosToPlay;
    private bool _disposedValue;

    public SerialSoundPlayer(ISoundPlayer soundPlayer)
    {
        _soundPlayer = soundPlayer;
        _soundPlayer.OnPlaybackComplete += PlayNext;
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
            _soundPlayer.Dispose();
            return;
        }

        _soundPlayer.Load(_audiosToPlay[_currentAudioId]);
        _soundPlayer.Play();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _soundPlayer.Dispose();
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

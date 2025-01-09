namespace TextBlade.Core.Audio;

public interface ISerialSoundPlayer
{
    void Play();
    void Stop();
    void Queue(string audioFile);
}

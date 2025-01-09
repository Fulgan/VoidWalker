namespace TextBlade.Core.Audio;

public interface ISoundPlayer
{
    public void Load(string audio);
    public void Play();
    public void Stop();
}

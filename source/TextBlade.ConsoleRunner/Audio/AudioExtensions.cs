using LibVLCSharp.Shared;

namespace TextBlade.ConsoleRunner.Audio;

public static class AudioExtensions
{
    public static AudioOutputChannel ToAudioOutputChannel(this string channel)
    {
        switch (channel.ToLower())
        {
            case "stereo": return AudioOutputChannel.Stereo;
            case "left": return AudioOutputChannel.Left;
            case "right": return AudioOutputChannel.Right;
            default: throw new ArgumentException($"Not sure what channel to map {channel} to", nameof(channel));
        }
    }
}

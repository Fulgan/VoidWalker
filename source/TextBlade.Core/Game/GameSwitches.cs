namespace TextBlade.Core.Game;

/// <summary>
/// Inspired by RPG Maker, each game has UNLIMITED switches. Do with them as you will.
/// </summary>
public class GameSwitches
{
    // Singleton-ish. The only/current instance.
    public static GameSwitches Switches { get; } = new();

    // The actual data. Public for serialization purposes.
    public Dictionary<string, bool> Data { get; } = new();

    public bool HasSwitch(string switchName)
    {
        return Data.ContainsKey(switchName);
    }

    public void Set(string switchName, bool value)
    {
        Data[switchName] = value;
    }

    public bool Get(string switchName)
    {
        if (!Data.ContainsKey(switchName))
        {
            throw new ArgumentNullException(switchName);
        }

        return Data[switchName];
    }
}

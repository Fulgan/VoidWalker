namespace TextBlade.Core.Game;

/// <summary>
/// Inspired by RPG Maker, each game has UNLIMITED switches. Do with them as you will.
/// </summary>
public class GameSwitches
{
    // Singleton-ish.
    public static GameSwitches Switches { get; } = new();

    private Dictionary<string, bool> _switches = new();

    public bool HasSwitch(string switchName)
    {
        return _switches.ContainsKey(switchName);
    }

    public void Set(string switchName, bool value)
    {
        _switches[switchName] = value;
    }

    public bool Get(string switchName)
    {
        if (!_switches.ContainsKey(switchName))
        {
            throw new ArgumentNullException(switchName);
        }

        return _switches[switchName];
    }
}

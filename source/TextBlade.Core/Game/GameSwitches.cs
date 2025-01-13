namespace TextBlade.Core.Game;

/// <summary>
/// Inspired by RPG Maker, each game has UNLIMITED switches. Do with them as you will.
/// </summary>
public class GameSwitches
{
    // Singleton-ish. The only/current instance.
    public static GameSwitches? Switches { get; set; } = new();

    // The actual data. Public for serialization purposes... This smells...
    public Dictionary<string, bool> Data { get; } = new();

    public static string GetCompletionSwitchForDungeon(string dungeonName)
    {
        dungeonName = dungeonName.Replace(" ", "").Replace("-", "").Replace("_", "");
        return $"DungeonComplete_{dungeonName}";
    }

    public bool Has(string switchName)
    {
        return Data.ContainsKey(switchName);
    }

    public void Set(string switchName, bool value)
    {
        Data[switchName] = value;
    }

    public bool Get(string switchName)
    {
        bool value;
        if (!Data.TryGetValue(switchName, out value))
        {
            throw new ArgumentException("Switch doesn't exist", nameof(switchName));
        }

        return value;
    }
}

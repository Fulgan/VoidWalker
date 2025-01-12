using TextBlade.Core.Inv;
using TextBlade.Core.Game;
using TextBlade.Core.Characters;

namespace TextBlade.Core.IO;

public class SaveData
{
    // Singleton-ish
    public static SaveData Current { get; private set; }

    public List<Character> Party { get; set; }
    public GameSwitches Switches { get; set; }
    public int Gold { get; set; }
    public string CurrentLocationId { get; set; }
    public Inventory Inventory { get; set; }

    public Dictionary<string, object>? LocationSpecificData { get; set; }
    // Who owns the location specific data? e.g. the dungeon, but you're in town right now
    public string LocationSpecificDataLocationId { get; set; }

    // For user code, i.e. game-specific things
    public Dictionary<string, object>? GameSpecificData { get; set; }

    public SaveData()
    {
        SaveData.Current = this;
    }
}

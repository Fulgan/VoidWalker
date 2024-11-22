using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.IO;

public struct SaveData
{
    public List<Character> Party { get; set; }
    public GameSwitches Switches { get; set; }
    public int Gold { get; set; }
    public int TotalGameTimeSeconds { get; set; }
    public string CurrentLocationId { get; set; }
    public Inventory Inventory { get; set; }

    public Dictionary<string, object>? LocationSpecificData { get; set; }

    // For user code, i.e. ggame-specific things
    public Dictionary<string, object> GameSpecificData { get; set; }
}

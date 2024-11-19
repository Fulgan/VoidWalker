using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.IO;

public struct SaveData
{
    public List<Character> Party { get; set; }
    public GameSwitches Switches { get; set; }
    public int Gold { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public int TotalGameTimeSeconds { get; set; }
    public string CurrentLocationId { get; set; }
}

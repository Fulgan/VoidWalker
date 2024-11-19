using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.IO;

public struct SaveData
{
    public List<Character> Party;
    public GameSwitches Switches;
    public int Gold;
    public Dictionary<string, int> Inventory;
    public int TotalGameTimeSeconds;
}

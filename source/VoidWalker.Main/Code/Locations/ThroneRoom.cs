using TextBlade.Core.UserCode;

namespace VoidWalker.Main.Code.Locations;

[LocationCode]
public class ThroneRoom
{
    public ThroneRoom()
    {
        TextBlade.Core.Game.GameSwitches.Switches.Set("MetKingSulayman", true);
    }
}
using TextBlade.Core.Game;
using TextBlade.Core.UserCode;

namespace VoidWalker.Main.Code.Locations;

public class ThroneRoom : LocationCodeBehind
{
    public ThroneRoom()
    {
        GameSwitches.Switches.Set(Switches.MetKingSulayman, true);
    }
}
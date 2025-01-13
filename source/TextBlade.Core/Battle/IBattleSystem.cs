using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Battle;

public interface IBattleSystem
{
    public Spoils Execute(SaveData saveData, Location currentLocation);
}

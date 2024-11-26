using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Game;

public interface IGame
{
    public void SetLocation(Location location);
    public Inventory Inventory { get; }
}

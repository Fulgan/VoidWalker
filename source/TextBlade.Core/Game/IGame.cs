using TextBlade.Core.Inv;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Game;

public interface IGame
{
    public void SetLocation(Location location);
    public Inventory Inventory { get; }
    public void Run();
}

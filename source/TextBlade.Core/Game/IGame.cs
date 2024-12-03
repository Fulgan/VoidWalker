using TextBlade.Core.Inv;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Game;

public interface IGame
{
    public Inventory Inventory { get; }
    public void SetLocation(Location location);
    public void Run();
}

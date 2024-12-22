using TextBlade.Core.Game;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Stubs;

public class GameStub : IGame
{
    public Location Location { get; private set; }
    
    public void Run()
    {
    }

    public void SetLocation(Location location)
    {
        this.Location = location;
    }
}

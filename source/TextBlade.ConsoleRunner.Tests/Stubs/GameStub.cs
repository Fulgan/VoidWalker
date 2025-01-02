using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner.Tests.Stubs;

public class GameStub : Game
{
    public GameStub(IConsole console) : base(console)
    {
        _saveData = new();
    }

    new public void SaveGame() => base.SaveGame();

    public Location CurrentLocation
    { 
        get { return _currentLocation; }
        set { _currentLocation = value; }
    }
}

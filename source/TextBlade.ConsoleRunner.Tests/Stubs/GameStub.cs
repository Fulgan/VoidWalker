using TextBlade.Core.IO;
using TextBlade.Core.Locations;
using TextBlade.Core.Services;

namespace TextBlade.ConsoleRunner.Tests.Stubs;

public class GameStub : Game
{
    public GameStub(IConsole console, ISoundPlayer soundPlayer) : base(console, soundPlayer)
    {
        _saveData = new();
    }

    new public void SaveGame() => base.SaveGame();

    public Location CurrentLocation { 
        get { return _currentLocation; }
        set { _currentLocation = value; }
    }
}

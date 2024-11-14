using System.Security.Cryptography.X509Certificates;
using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Now this here, this is yer basic game. Keeps track of the current location, party, etc.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game
{
    private Location _currentLocation = null!;
    private bool _isRunning = true;
    private List<Character> _party = new();

    public void Run()
    {
        var runner = new NewGameRunner(this);
        runner.ShowGameIntro();
        _party = runner.CreateParty();

        var startLocationId = runner.GetStartingLocationId();
        _currentLocation = new ChangeLocationCommand(startLocationId).Execute();

        while (_isRunning)
        {
            LocationDisplayer.ShowLocation(_currentLocation);
            var command = InputProcessor.PromptForAction(_currentLocation);
            // If it returned a new location, fantastico, adopt it.
            _currentLocation = command.Execute() ?? _currentLocation;         
        }
    }
}

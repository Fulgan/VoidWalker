using Spectre.Console;
using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.Game;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Now this here, this is yer basic game. Keeps track of the current location, party, etc.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game : IGame
{
    private Location _currentLocation = null!;
    private bool _isRunning = true;
    private List<Character> _party = new();

    public static IGame Current { get; private set; }

    public Game()
    {
        Current = this;
    }

    public void SetLocation(Location location)
    {
        _currentLocation = location;
    }

    public void Run()
    {
        var runner = new NewGameRunner(this);
        runner.ShowGameIntro();
        _party = runner.CreateParty();

        var startLocationId = runner.GetStartingLocationId();
        new ChangeLocationCommand(startLocationId).Execute(this, _party);

        // Don't execute code if we stay in the same location, e.g. press enter or "help" - only execute code
        // if the location changed. Fixes a bug where spamming enter keeps adding the same location over and over ...
        Location? previousLocation = null;

        while (_isRunning)
        {
            if (previousLocation != _currentLocation)
            {
                CodeBehindRunner.ExecuteLocationCode(_currentLocation);
            }

            LocationDisplayer.ShowLocation(_currentLocation);
            var command = InputProcessor.PromptForAction(_currentLocation);
            previousLocation = _currentLocation;
            
            var result = command.Execute(this, _party);

            foreach (var message in result)
            {
                AnsiConsole.MarkupLine(message);
            }
        }
    }
}

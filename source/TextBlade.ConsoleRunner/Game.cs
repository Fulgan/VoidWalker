using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Characters;
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
        SetLocationTo(startLocationId);

        while (_isRunning)
        {
            LocationDisplayer.ShowLocation(_currentLocation);
            var destinationId = InputProcessor.PromptForDestination(_currentLocation);
            // Assume it's valid
            SetLocationTo(destinationId);
        }
    }

    internal void SetLocationTo(string locationId)
    {
        var locationName = locationId.ToString().Replace('/', Path.DirectorySeparatorChar);
        var locationPath = Path.Join("Content", "Locations", $"{locationName}.json");
        if (!File.Exists(locationPath))
        {
            throw new InvalidOperationException($"{locationPath} doesn't seem to exist!");
        }

        var locationData = Serializer.Deserialize<Location>(File.ReadAllText(locationPath));
        _currentLocation = locationData;
    }

    
}

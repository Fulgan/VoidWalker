using TextBlade.Core.Characters;
using TextBlade.Core.IO;
using Region = TextBlade.Core.Locations.Region;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Now this here, this is yer basic game. Keeps track of the current region, party, etc.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game
{
    private Region _currentLocation = null!;
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
            ShowCurrentLocation();
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

        var locationData = Serializer.Deserialize<Region>(File.ReadAllText(locationPath));
        _currentLocation = locationData;
    }

    internal void ShowCurrentLocation()
    {
        if (_currentLocation == null)
        {
            throw new InvalidOperationException("Current location is null!");
        }

        Console.WriteLine($"You are in {_currentLocation.Name}: {_currentLocation.Description}");
        Console.WriteLine($"You can go to {_currentLocation.ReachableRegions.Count} places:");
        
        int i = 0;
        foreach (var region in _currentLocation.ReachableRegions)
        {
            i++;
            Console.WriteLine($"    {i}: {region.Description}");
        }

        Console.Write("Enter the number of your destination: ");
        var answer = int.Parse(Console.ReadLine().Trim());
        // Assume it's valid
        var destination = _currentLocation.ReachableRegions[answer - 1];

        // Assume it's valid
        SetLocationTo(destination.Id);
    }
}

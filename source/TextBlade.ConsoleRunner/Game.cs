using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using TextBlade.Core.IO;
using Region = TextBlade.Core.Locations.Region;

namespace TextBlade.ConsoleRunner;

public class Game
{
    private JObject _gameJson = null!;
    private Region _currentLocation = null!;

    public void Run()
    {
        ShowGameIntro();
        SetStartingLocation();

        while (true) {
            ShowCurrentLocation();
        }
    }

    internal void ShowGameIntro()
    {
        var gameJsonPath = Path.Join("Content", "game.json");

        if (!File.Exists(gameJsonPath))
        {
            throw new InvalidOperationException("Content/game.json file is missing!");
        }

        var gameJsonContents = File.ReadAllText(gameJsonPath);
        _gameJson = JsonConvert.DeserializeObject(gameJsonContents) as JObject;
        var gameName = _gameJson["GameName"];
        AnsiConsole.MarkupLine($"[#fff]Welcome to[/] [#f00]{gameName}[/]!");
    }

    internal void SetStartingLocation()
    {
        if (!_gameJson.ContainsKey("StartingLocationId"))
        {
            throw new InvalidOperationException("Your game.json doesn't have a StartingLocationId attribute!");
        }

        SetLocationTo(_gameJson["StartingLocationId"].ToString());
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

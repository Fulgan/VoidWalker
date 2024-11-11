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
        GetStartingLocation();
        ShowCurrentLocation();
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

    internal void GetStartingLocation()
    {
        if (!_gameJson.ContainsKey("StartingLocationId"))
        {
            throw new InvalidOperationException("Your game.json doesn't have a StartingLocationId attribute!");
        }

        var startingLocationName = _gameJson["StartingLocationId"].ToString().Replace('/', Path.DirectorySeparatorChar);
        var locationPath = Path.Join("Content", "Locations", $"{startingLocationName}.json");
        if (!File.Exists(locationPath))
        {
            throw new InvalidOperationException($"{locationPath} doesn't seem to exist!");
        }

        var startingLocationData = Serializer.Deserialize<Region>(File.ReadAllText(locationPath));
        _currentLocation = startingLocationData;
        ;
    }

    internal void ShowCurrentLocation()
    {
        if (_currentLocation == null)
        {
            throw new InvalidOperationException("Current location is null!");
        }

        Console.WriteLine($"You are in {_currentLocation.Name}: {_currentLocation.Description}");
    }
}

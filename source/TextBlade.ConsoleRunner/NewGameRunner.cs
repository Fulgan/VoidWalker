using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Terribly named; the thing that does all your new-game stuff.
/// </summary>
public class NewGameRunner(Game game)
{
    private JObject? _gameJson;
    private JObject GameJson
    {
        get => _gameJson ?? throw new Exception("No game loaded");
        set => _gameJson = value;
    }

    private Game _game = game;

    public List<Character> CreateParty()
    {
        var partyData = GameJson["StartingParty"] as JArray ?? throw new Exception("");
        var party = Serializer.DeserializeParty(partyData);
        return party;
    }

    
    public void ShowGameIntro()
    {
        var gameJsonPath = Path.Join("Content", "game.json");

        if (!File.Exists(gameJsonPath))
        {
            throw new InvalidOperationException("Content/game.json file is missing!");
        }

        AnsiConsole.Background = Color.Black;
        var gameJsonContents = File.ReadAllText(gameJsonPath);
        if (JsonConvert.DeserializeObject(gameJsonContents) is not JObject jObject)
            throw new Exception("game.json is not a valid JSON object!");
        GameJson = jObject;
        var gameName = GameJson["GameName"];
        AnsiConsole.MarkupLine($"[white]Welcome to[/] [red]{gameName}[/]!");
    }

    public string GetStartingLocationId()
    {
        if (!GameJson.TryGetValue("StartingLocationId", out var locationIdToken))
        {
            throw new InvalidOperationException("Your game.json doesn't have a StartingLocationId attribute!");
        }

        return locationIdToken.ToString();
    }

}

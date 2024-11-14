using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Terribly named; the thing that does all your new-game stuff.
/// </summary>
public class NewGameRunner
{
    private JObject _gameJson = null!;
    private Game _game = null!;

    public NewGameRunner(Game game)
    {
        _game = game;
    }

    public List<Character> CreateParty()
    {
        var partyData = _gameJson["StartingParty"] as JArray;
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

        var gameJsonContents = File.ReadAllText(gameJsonPath);
        _gameJson = JsonConvert.DeserializeObject(gameJsonContents) as JObject;
        var gameName = _gameJson["GameName"];
        AnsiConsole.MarkupLine($"[#fff]Welcome to[/] [#f00]{gameName}[/]!");
    }

    public string GetStartingLocationId()
    {
        if (!_gameJson.ContainsKey("StartingLocationId"))
        {
            throw new InvalidOperationException("Your game.json doesn't have a StartingLocationId attribute!");
        }

        var regionId = _gameJson["StartingLocationId"].ToString();
        return regionId;
    }

}

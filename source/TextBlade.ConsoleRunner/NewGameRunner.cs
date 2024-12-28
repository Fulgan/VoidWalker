using Newtonsoft.Json.Linq;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Terribly named; the thing that does all your new-game stuff.
/// </summary>
public class NewGameRunner(JObject gameJson)
{
    private JObject _gameJson = gameJson;

    public List<Character> CreateParty()
    {
        if (!_gameJson.TryGetValue("StartingLocationId", out var locationIdToken))
        {
            throw new InvalidOperationException("Your game.json doesn't have a StartingLocationId attribute!");
        }

        var partyData = _gameJson["StartingParty"] as JArray ?? throw new Exception("");
        var party = Serializer.DeserializeParty(partyData);
        return party;
    }

    public string GetStartingLocationId()
    {
        if (!_gameJson.TryGetValue("StartingLocationId", out var locationIdToken))
        {
            throw new InvalidOperationException("Your game.json doesn't have a StartingLocationId attribute!");
        }

        return locationIdToken.ToString();
    }

}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

/// <summary>
/// Unlike most of the other commands, handles all the (fighting) logic and user input/response internally.
/// </summary>
public class FightCommand : ICommand
{
    private readonly List<Monster> _monsters;
    private static JObject _allMonstersData; // name => stats

    static FightCommand()
    {
        // By convention for now
        var jsonPath = Path.Join("Content", "Data", "Monsters.json");
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"{jsonPath} doesn't seem to exist.");
        }

        var jsonContent = File.ReadAllText(jsonPath);
        _allMonstersData = JsonConvert.DeserializeObject(jsonContent) as JObject;
        if (_allMonstersData == null)
        {
            throw new InvalidOperationException($"{jsonPath} doesn't seem to be valid JSON.");
        }

        ;
    }

    public FightCommand(List<string> monsterNames)
    {
        ;
    }

    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        // Problem: we don't have access to AnsiConsole in this layer. Nor can we wait for the Game class
        // to process it, because it's an interactive battle. That ... sucks...
        return [];
    }
}

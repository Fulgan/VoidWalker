using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

/// <summary>
/// Unlike most of the other commands, handles all the (fighting) logic and user input/response internally.
/// </summary>
public class FightCommand : ICommand, IBattleCommand
{
    private const string CommentsInJsonRegex = @"(//.*)";
    public const string VictoryMessage = "Victory! You gained {0} gold!";
    public const string DefeatMessage = "Defeat!";
    public int TotalGold => _monsters.Sum(m => m.Gold);

    private readonly List<Monster> _monsters = new();
    private static JObject _allMonstersData; // name => stats

    public bool IsVictory { get; private set; }

    static FightCommand()
    {
        // By convention for now
        var jsonPath = Path.Join("Content", "Data", "Monsters.json");
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"{jsonPath} doesn't seem to exist.");
        }

        // Remove comments...
        var rawLines = File.ReadAllLines(jsonPath);
        var commentlessText = new StringBuilder();
        foreach (var line in rawLines)
        {
            commentlessText.Append(Regex.Replace(line, CommentsInJsonRegex, string.Empty));
        }

        var jsonContent = commentlessText.ToString();
        _allMonstersData = JsonConvert.DeserializeObject(jsonContent) as JObject;
        if (_allMonstersData == null)
        {
            throw new InvalidOperationException($"{jsonPath} doesn't seem to be valid JSON.");
        }
    }

    public FightCommand(List<string> monsterNames)
    {
        foreach (var name in monsterNames)
        {
            var data = _allMonstersData[name];
            if (data == null)
            {
                throw new ArgumentException($"Can't find monster data for {name}");
            }

            var health = data.Value<int>("Health");
            var strength = data.Value<int>("Strength");
            var toughness = data.Value<int>("Toughness");
            var weakness = data.Value<string?>("Weakness") ?? string.Empty;
            var gold = data.Value<int>("Gold");
            var monster = new Monster(name, health, strength, toughness, gold, weakness);
            _monsters.Add(monster);
        }
    }

    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        // Problem: we don't have access to AnsiConsole in this layer. Nor can we wait for the Game class
        // to process it, because it's an interactive battle. That ... sucks...
        var isPartyWipedOut = () => party.TrueForAll(p => p.CurrentHealth <= 0);
        var areMonstersDefeated = () => _monsters.TrueForAll(m => m.CurrentHealth <= 0);
        var isBattleOver = () => isPartyWipedOut() || areMonstersDefeated();
        var characterTurnProcessor = new CharacterTurnProcessor(party, _monsters);

        while (!isBattleOver())
        {
            var monstersStatus = string.Join(", ", _monsters.Select(m => $"{m.Name}: {m.CurrentHealth}/{m.TotalHealth} health"));
            yield return $"You face: [{Colours.Highlight}]{monstersStatus}[/]";
            var partyStatus = string.Join(", ", party.Select(m => $"{m.Name}: {m.CurrentHealth}/{m.TotalHealth} health"));
            yield return $"Your party: [{Colours.Highlight}]{partyStatus}[/]";

            foreach (var character in party)
            {
                if (character.CurrentHealth <= 0)
                {
                    continue;
                }

                yield return characterTurnProcessor.ProcessTurnFor(character);
            }

            foreach (var monster in _monsters)
            {
                if (monster.CurrentHealth <= 0)
                {
                    continue;
                }

                yield return new BasicMonsterAi(party).ProcessTurnFor(monster);
            }

            foreach (var e in _monsters)
            {
                if (e.CurrentHealth <= 0)
                {
                    continue;
                }
                
                foreach (var message in e.OnRoundComplete())
                {
                    yield return message;
                }
            }

            foreach (var e in party)
            {
                if (e.CurrentHealth <= 0)
                {
                    continue;
                }
                
                foreach (var message in e.OnRoundComplete())
                {
                    yield return message;
                }
            }
        }

        if (isPartyWipedOut())
        {
            this.IsVictory = false;
            yield return DefeatMessage;
        }
        else if (areMonstersDefeated())
        {
            this.IsVictory = true;
            yield return string.Format(VictoryMessage, TotalGold);
        }
        else
        {
            throw new InvalidOperationException("Undeterminable battle status!");
        }
    }
}

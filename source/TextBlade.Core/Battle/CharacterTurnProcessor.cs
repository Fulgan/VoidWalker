using TextBlade.Core.Characters;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

public class CharacterTurnProcessor
{
    private readonly List<Monster> _monsters;
    private readonly SaveData _saveData;
    private readonly IConsole _console;

    private readonly char[] validInputs = ['a', 'i', 's', 'd'];

    public CharacterTurnProcessor(IConsole console, SaveData saveData, List<Monster> monsters)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(saveData);
        ArgumentNullException.ThrowIfNull(monsters);
        
        _console = console;
        _saveData = saveData;
        _monsters = monsters;
    }

    internal void ProcessTurnFor(Character character)
    {
        char input = ' ';

        while (!validInputs.Contains(input))
        {
            _console.WriteLine($"{character.Name}'s turn. Pick an action:");
            _console.WriteLine($"    [{Colours.Command}]A:[/] attack");
            _console.WriteLine($"    [{Colours.Command}]S:[/] skill");
            _console.WriteLine($"    [{Colours.Command}]D:[/] defend");
            _console.WriteLine($"    [{Colours.Command}]I:[/] item");
            input = _console.ReadKey();
        }

        IEnumerable<Entity> targets; 

        switch(input)
        {
            case 'a':
                targets = [PickTargetMonster()];
                new AttackExecutor(_console).Attack(character, targets.First() as Monster);
                break;
            case 'd':
                character.Defend(_console);
                break;
            case 's':
                var skill = PickSkillFor(character);
                targets = PickTargetsFor(skill);
                new SkillApplier(_console).Apply(character, skill, targets);
                break;
            case 'i':
                new ShowInventoryCommand(_console, true).Execute(_saveData);
                break;
            default:
                _console.WriteLine("Invalid input!");
                break;
        }
    }

    private IEnumerable<Entity>? PickTargetsFor(Skill skill)
    {
        switch (skill.Target)
        {
            case null:
            case "":
            case "Single":
            case "Enemy":
            case "Monster": // TODO: it's Enemy now, will switch
                return [PickTargetMonster()];
            case "All":
                return _monsters.Where(m => m.CurrentHealth > 0);
            case "Character":
                return [PickTargetCharacter()];
            case "Party":
                return _saveData.Party;
            default:
                throw new InvalidOperationException($"TextBlade doesn't know how to pick a target of type: {skill.Target ?? "(null)"}");
        }       
    }

    /// <summary>
    /// Lets the player pick from any character, dead or alive.
    /// </summary>
    /// <returns></returns>
    private Character PickTargetCharacter()
    {
        var validTargets = _saveData.Party;
        return PickFromList(validTargets);
    }

    private Entity PickTargetMonster()
    {
        var validTargets = _monsters.Where(m => m.CurrentHealth > 0);
        if (!validTargets.Any())
        {
            throw new InvalidOperationException("Character's turn when all monsters are dead.");
        }

        // Refactor: we use this "pick a valid int from this list" everywhere. DRY.
        return PickFromList(validTargets);
    }

    private T PickFromList<T>(IEnumerable<T> items)
    {
        _console.WriteLine("Pick a target:");

        for (int i = 0; i < items.Count(); i++)
        {
            var item = items.ElementAt(i);
            _console.WriteLine($"    {i + 1}: {item}");
        }

        int target;
        while (!int.TryParse(_console.ReadKey().ToString(), out target) || target == 0 || target > items.Count())
        {
            _console.WriteLine($"That's not a valid number! Enter a number from 1 to {items.Count()}: ");
        }

        return items.ElementAt(target - 1);
    }

    private Skill PickSkillFor(Character character)
    {
        _console.WriteLine("Pick a skill: ");
        var skill = PickFromList(character.Skills);
        
        while (character.CurrentSkillPoints < skill.Cost)
        {
            _console.WriteLine($"{character.Name} has {character.CurrentSkillPoints} skill points, which isn't enough for {skill.Name}.");
            _console.WriteLine("Pick a skill: ");
            skill = PickFromList(character.Skills);
        }

        return skill;
    }
}

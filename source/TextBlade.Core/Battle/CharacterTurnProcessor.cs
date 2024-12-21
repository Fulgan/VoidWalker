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

    internal bool ProcessTurnFor(Character character)
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
                var target = PickTargetMonster();
                
                if (target == null)
                {
                    return false;
                }
                
                new AttackExecutor(_console).Attack(character, target as Monster);
                break;
            case 'd':
                character.Defend(_console);
                break;
            case 's':
                var skill = PickSkillFor(character);
                if (skill == null)
                {
                    return false;
                }

                targets = PickTargetsFor(skill);
                
                if (targets == null)
                {
                    return false;
                }

                new SkillApplier(_console).Apply(character, skill, targets);
                break;
            case 'i':
                var command = new ShowInventoryCommand(_console, true);
                return command.Execute(_saveData);
            default:
                _console.WriteLine("Invalid input!");
                break;
        }

        return true;
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
                var monster = PickTargetMonster();
                return monster != null ? [monster] : null;
            case "All":
                return _monsters.Where(m => m.CurrentHealth > 0);
            case "Character":
                var character = PickTargetCharacter();
                return character != null ? [character] : null;
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
    private Character? PickTargetCharacter()
    {
        var validTargets = _saveData.Party;
        return PickFromList(validTargets);
    }

    private Entity? PickTargetMonster()
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
        _console.WriteLine("Pick a target, or 0 or b to cancel:");

        for (int i = 0; i < items.Count(); i++)
        {
            var item = items.ElementAt(i);
            _console.WriteLine($"    {i + 1}: {item}");
        }

        var target = 0;
        while (target == 0)
        {        
            var rawInput = _console.ReadKey();
            if (rawInput == '0' || rawInput == 'b')
            {
                _console.WriteLine($"[{Colours.Cancel}]Cancelling.[/]");
                return default;
            }

            if (!int.TryParse(rawInput.ToString(), out target))
            {
                _console.WriteLine("That's not a valid number.");
            }

            if (target < 1 || target > items.Count())
            {
                _console.WriteLine($"Please enter a number between {1} and {items.Count()}.");
                target = 0;
             }
        }

        return items.ElementAt(target - 1);
    }

    private Skill? PickSkillFor(Character character)
    {
        _console.WriteLine("Pick a skill: ");
        var skill = PickFromList(character.Skills);
        if (skill == null)
        {
            // Cancelling
            return null;
        }
        
        while (skill != null && character.CurrentSkillPoints < skill.Cost)
        {
            _console.WriteLine($"{character.Name} has {character.CurrentSkillPoints} skill points, which isn't enough for {skill.Name}.");
            _console.WriteLine("Pick a skill: ");
            skill = PickFromList(character.Skills);
        }

        return skill;
    }
}

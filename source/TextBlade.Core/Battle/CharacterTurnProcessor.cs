using System.ComponentModel.DataAnnotations;
using System.Text;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Battle;

public class CharacterTurnProcessor
{
    private readonly List<Monster> _monsters;

    public CharacterTurnProcessor(List<Character> party, List<Monster> monsters)
    {
        _monsters = monsters;
    }

    internal void ProcessTurnFor(Character character)
    {
        Console.WriteLine($"{character.Name}'s turn. Pick an action: [a]ttack, [s]kill, or [d]efend");
        var input = Console.ReadKey();
        int target; // not going to work for healing skills

        switch(input.KeyChar)
        {
            case 'a':
            case 'A':
                target = PickTarget();
                Console.WriteLine(Attack(character, target));
                return;
            case 'd':
            case 'D':
                character.Defend();
                Console.WriteLine($"{character.Name} defends!");
                return;
            case 's':
            case 'S':
                // Assumes you get back a valid skill: something you have SP for.
                var skill = PickSkillFor(character);
                target = PickTarget();
                // Depending on the skill, the target is an instance of Character or Monster.
                // For now, assume monster.
                Console.WriteLine(SkillApplier.Apply(character, skill, _monsters[target - 1]));
                return;
        }
    }

    private int PickTarget()
    {
        Console.WriteLine("Pick a target:");
        for (int i = 0; i < _monsters.Count; i++)
        {
            var monster = _monsters[i];
            Console.WriteLine($"    {i+1}: {monster.Name} ({monster.CurrentHealth}/{monster.TotalHealth} health)");
        }

        var target = int.Parse(Console.ReadKey().KeyChar.ToString());
        return target;
    }

    private static Skill PickSkillFor(Character character)
    {
        Console.WriteLine("Pick a skill:");
        for (int i = 0; i < character.Skills.Count; i++)
        {
            Console.WriteLine($"    {i+1}: {character.SkillNames[i]}");
        }

        var target = int.Parse(Console.ReadKey().KeyChar.ToString());
        // Assume target is valid
        return character.Skills[target - 1];
    }

    private string Attack(Character character, int target)
    {
        // Assume target number is legit
        var targetMonster = _monsters[target - 1];
        var message = new StringBuilder();
        message.Append($"{character.Name} attacks {targetMonster.Name}! ");
        
        var damage = character.Strength - targetMonster.Toughness;
        targetMonster.Damage(damage);
        
        var damageAmount = damage <= 0 ? "NO" : damage.ToString();
        message.Append($"{damageAmount} damage!");
        if (targetMonster.CurrentHealth <= 0)
        {
            message.Append($"{targetMonster.Name} DIES!");
        }
        
        return message.ToString();
    }
}

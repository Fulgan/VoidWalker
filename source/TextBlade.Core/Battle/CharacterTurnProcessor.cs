using System.ComponentModel.DataAnnotations;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Battle;

public class CharacterTurnProcessor
{
    private readonly List<Character> _party;
    private readonly List<Monster> _monsters;

    public CharacterTurnProcessor(List<Character> party, List<Monster> monsters)
    {
        _party = party;
        _monsters = monsters;
    }

    internal void ProcessTurnFor(Character character)
    {
        Console.WriteLine($"{character.Name}'s turn. Pick an action: [a]ttack, [d]efend");
        var input = Console.ReadKey();
        switch(input.KeyChar)
        {
            case 'a':
            case 'A':
                PickTargetAndAttack(character);
                return;
            case 'd':
            case 'D':
                character.Defend();
                Console.WriteLine($"{character.Name} defends!");
                return;
        }
    }

    private void PickTargetAndAttack(Character character)
    {
        Console.WriteLine("Pick a target:");
        for (int i = 0; i < _monsters.Count; i++)
        {
            var monster = _monsters[i];
            Console.WriteLine($"    {i+1}: {monster.Name} ({monster.CurrentHealth}/{monster.TotalHealth} health)");
        }

        var target = int.Parse(Console.ReadKey().KeyChar.ToString());
        // Assume target number is legit
        var targetMonster = _monsters[target - 1];
        var message = $"{character.Name} attacks {targetMonster.Name}! ";
        
        var damage = character.Strength - targetMonster.Toughness;
        targetMonster.Damage(damage);
        
        var damageAmount = damage <= 0 ? "NO" : damage.ToString();
        message += $"{damageAmount} damage!";
        var deathMessage = targetMonster.CurrentHealth <= 0 ? $"{targetMonster.Name} DIES!" : "";
        Console.WriteLine($"{message} {deathMessage}");
    }
}

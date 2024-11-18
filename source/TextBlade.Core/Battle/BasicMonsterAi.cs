using TextBlade.Core.Characters;

namespace TextBlade.Core.Battle;

public class BasicMonsterAi
{
    private readonly List<Character> _party;

    public BasicMonsterAi(List<Character> party)
    {
        _party = party.Where(p => p.CurrentHealth > 0).ToList();
    }
    
    public void ProcessTurnFor(Monster monster)
    {
        var target = _party[Random.Shared.Next(0, _party.Count)];

        var damage = monster.Attack(target);
        var message = $"{monster.Name} attacks {target.Name} for {damage} damage! ";
        if (target.CurrentHealth <= 0)
        {
            message += $"{target.Name} DIES! Oh no!";
        }
        Console.WriteLine(message);
    }    
}

using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

public class BasicMonsterAi
{
    private readonly List<Character> _party;

    public BasicMonsterAi(List<Character> party)
    {
        _party = party.Where(p => p.CurrentHealth > 0).ToList();
    }
    
    public string ProcessTurnFor(Monster monster)
    {
        if (_party.Count == 0)
        {
            // Wiped out, nothing to do
            return string.Empty;
        }

        var target = _party[Random.Shared.Next(0, _party.Count)];

        var damage = monster.Attack(target);
        var message = $"{monster.Name} attacks {target.Name} for [{Colours.Highlight}]{damage}[/] damage! ";
        if (target.CurrentHealth <= 0)
        {
            message += $"{target.Name} [{Colours.Highlight}]DIES![/] Oh no!";
        }
        return message;
    }    
}

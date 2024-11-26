using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands.Display;

public class ShowPartyStatusCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        yield return "Party status:";

        foreach (var member in party)
        {
            yield return $"    {member.Name}: {member.CurrentHealth}/{member.TotalHealth} health";
        }
    }
}

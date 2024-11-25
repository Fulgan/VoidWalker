using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class ShowPartyStatusCommand : ICommand
{
    public async IAsyncEnumerable<string> Execute(IGame game, List<Character> party)
    {
        yield return "Party status:";

        foreach (var member in party)
        {
            yield return $"    {member.Name}: {member.CurrentHealth}/{member.TotalHealth} health";
        }
    }
}

using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class ShowPartyStatusCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        var strings = new List<string>
        {
            "Party status:"
        };

        foreach (var member in party)
        {
            strings.Add($"    {member.Name}: {member.CurrentHealth}/{member.TotalHealth} health");
        }

        return strings;
    }
}

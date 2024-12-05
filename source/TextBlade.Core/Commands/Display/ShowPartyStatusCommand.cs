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
            var equipment = string.Join(", ", member.Equipment.Values.Select(e => $"{e.Name}: {e}"));

            yield return $"    {member.Name}: {member.CurrentHealth}/{member.TotalHealth} health.";
            yield return $"        Equipment: {(string.IsNullOrWhiteSpace(equipment) ? "nothing" : equipment)}";
        }
    }
}

using System;
using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class ShowPartyStatusCommand : Command
{
    public override Location? Execute(List<Character> party)
    {
        Console.WriteLine("Party status:");
        foreach (var member in party)
        {
            Console.WriteLine($"    {member.Name}: {member.CurrentHealth}/{member.TotalHealth} health");
        }

        return null;
    }
}

using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class SleepAtInnCommand : Command
{
    private int _innCost = 0;

    public SleepAtInnCommand(int innCost)
    {
        _innCost = innCost;
    }

    public override Location? Execute(List<Character> party)
    {
        // Check if we have enough gold. Subtract if we do!

        foreach (var character in party)
        {
            character.CurrentHealth = character.TotalHealth;
        }

        Console.WriteLine("You sleep at the inn. All party members have recovered to full health!");

        return null;
    }
}

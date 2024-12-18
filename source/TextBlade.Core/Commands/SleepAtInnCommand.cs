using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class SleepAtInnCommand : ICommand
{
    private readonly IConsole _console;
    private readonly int _innCost = 0;

    public SleepAtInnCommand(IConsole console, int innCost)
    {
        _console = console;
        _innCost = innCost;
    }

    public void Execute(SaveData saveData)
    {
        // Check if we have enough gold BEFORE THIS POINT. Subtract if we do!
        foreach (var character in saveData.Party)
        {
            character.FullyHeal();
        }

        _console.WriteLine($"You sleep at the inn. All party members have recovered to [{Colours.Highlight}]full health[/]!");
    }
}

using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class SleepAtInnCommand : ICommand
{
    private readonly IConsole _console;
    private readonly int _innCost;

    public SleepAtInnCommand(IConsole console, int innCost)
    {
        _console = console;
        _innCost = innCost;
    }

    public void Execute(SaveData saveData)
    {
        if (saveData.Gold < _innCost)
        {
            _console.WriteLine("You don't have enough gold!");
            return;
        }

        saveData.Gold -= _innCost;

        foreach (var character in saveData.Party)
        {
            character.FullyHeal();
        }

        _console.WriteLine($"You sleep at the inn. All party members have recovered to [{Colours.Highlight}]full health[/]!");
    }
}

using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class SleepAtInnCommand : ICommand
{
    private readonly int _innCost;

    public SleepAtInnCommand(int innCost)
    {
        _innCost = innCost;
    }

    public bool Execute(IConsole console, SaveData saveData)
    {
        if (saveData.Gold < _innCost)
        {
            console.WriteLine("You don't have enough gold!");
            return false;
        }

        saveData.Gold -= _innCost;

        foreach (var character in saveData.Party)
        {
            character.FullyHeal();
        }

        console.WriteLine($"You sleep at the inn. All party members have recovered to [{Colours.Highlight}]full health[/]!");
        return true;
    }
}

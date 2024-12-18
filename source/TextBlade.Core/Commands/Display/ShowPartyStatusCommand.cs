using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands.Display;

public class ShowPartyStatusCommand : ICommand
{
    private readonly IConsole _console;

    public ShowPartyStatusCommand(IConsole console)
    {
        _console = console;
    }

    public void Execute(SaveData saveData)
    {
        _console.WriteLine("Party status:");

        foreach (var member in saveData.Party)
        {
            var equipment = string.Join(", ", member.Equipment.Values.Select(e => $"{e.Name}: {e}"));

            _console.WriteLine($"    {member}");
            _console.WriteLine($"        Equipment: {(string.IsNullOrWhiteSpace(equipment) ? "nothing" : equipment)}");
        }
    }
}

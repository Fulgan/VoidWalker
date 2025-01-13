using TextBlade.Core.Characters;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands.Display;

public class TalkCommand : ICommand
{
    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
    {
       if (!currentLocation.Npcs.Any())
        {
            console.WriteLine("There's nobody here to talk to!");
            return false;
        }

        if (currentLocation.Npcs.Count() == 1)
        {
            TalkTo(console, currentLocation.Npcs[0]);
            return true;
        }

        var index = 0;
        while (index == 0)
        {        
            console.WriteLine("Talk to who?");

            var i = 0;
            foreach (var npc in currentLocation.Npcs)
            {
                i++;
                console.WriteLine($"    {i}: {npc.Name}");
            }

            var rawInput = console.ReadKey();
            if (rawInput == '0')
            {
                console.WriteLine($"[{Colours.Cancel}]Cancelling.[/]");
                return false;
            }

            if (!int.TryParse(rawInput.ToString(), out index))
            {
                console.WriteLine("That's not a valid number.");
                continue;
            }

            if (index < 1 || index > currentLocation.Npcs.Count())
            {
                console.WriteLine($"Please enter a number between {1} and {currentLocation.Npcs.Count()}.");
                index = 0;
                continue;
            }

            TalkTo(console, currentLocation.Npcs[index - 1]);
            return true;
        }

        return true; // Makes compiler go brrr
    }

    private void TalkTo(IConsole console, Npc npc)
    {
        console.WriteLine($"{npc.Name} says: {npc.Speak()}");
    }
}

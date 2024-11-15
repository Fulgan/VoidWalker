using TextBlade.Core.Commands;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner.IO;

public static class InputProcessor
{
    public static Command PromptForAction(Location currentLocation)
    {
        Console.Write("Enter the number of your destination: ");
        int destinationOption = -1;
        var rawResponse = Console.ReadLine().Trim().ToLowerInvariant();
        
        if (int.TryParse(rawResponse, out destinationOption))
        {
            // Assume it's valid
            var destination = currentLocation.LinkedLocations[destinationOption - 1];
            return new LoadLocationDataCommand(destination.Id);
        }

        // OK, so it's not a destination. Maybe it's a command, like QUIT.
        // We could give priority to the current location to process the command. But even if it does,
        // it may still return null, so we won't know. Hmm. Leave this for now...

        // If you update this, update the help listing in ShowHelpCommand.
        switch (rawResponse)
        {
            case "quit":
            case "q":
                return new QuitGameCommand();
            case "help":
            case "h":
            case "?":
                return new ShowHelpCommand();
            default:
                break; // More processing to do below
        }

        // Assume it's some special command that the location handles. That doesn't change location.
        var command = currentLocation.GetCommandFor(rawResponse);
        return command ?? new DoNothingCommand();
    }
}

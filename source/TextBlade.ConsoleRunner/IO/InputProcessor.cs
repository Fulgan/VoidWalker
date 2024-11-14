using TextBlade.Core.Commands;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner.IO;

public static class InputProcessor
{
    public static Command PromptForAction(Location currentLocation)
    {
        Console.Write("Enter the number of your destination: ");
        int destinationOption = -1;
        var rawResponse = Console.ReadLine().Trim();
        if (int.TryParse(rawResponse, out destinationOption))
        {
            // Assume it's valid
            var destination = currentLocation.LinkedLocations[destinationOption - 1];
            return new ChangeLocationCommand(destination.Id);
        }

        // Assume it's some special command that the location handles. That doesn't change location.
        var command = currentLocation.GetCommandFor(rawResponse);
        return command ?? new DoNothingCommand();
    }
}

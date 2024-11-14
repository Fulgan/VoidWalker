using TextBlade.Core.Commands;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner.IO;

public static class InputProcessor
{
    public static Command PromptForAction(Location currentLocation)
    {
        Console.Write("Enter the number of your destination: ");
        int destinationOption = -1;
        if (int.TryParse(Console.ReadLine().Trim(), out destinationOption))
        {
            // Assume it's valid
            var destination = currentLocation.LinkedLocations[destinationOption - 1];
            return new ChangeLocationCommand(destination.Id);
        }

        return null;
    }
}

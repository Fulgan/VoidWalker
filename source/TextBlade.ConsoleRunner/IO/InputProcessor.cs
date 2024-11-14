using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner.IO;

public static class InputProcessor
{
    public static string PromptForDestination(Location currentLocation)
    {
        Console.Write("Enter the number of your destination: ");
        var answer = int.Parse(Console.ReadLine().Trim());
        // Assume it's valid
        var destination = currentLocation.LinkedLocations[answer - 1];
        return destination.Id;
    }
}

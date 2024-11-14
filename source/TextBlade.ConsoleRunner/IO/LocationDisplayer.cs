using Spectre.Console;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner;

public static class LocationDisplayer
{
    /// <summary>
    /// Show a location.
    /// </summary>
    public static void ShowLocation(Location currentLocation)
    {
        if (currentLocation == null)
        {
            throw new InvalidOperationException("Current location is null!");
        }

        AnsiConsole.MarkupLine($"You are in [#00a]{currentLocation.Name}: {currentLocation.Description}[/]");
        AnsiConsole.MarkupLine($"You can go to [#00a]{currentLocation.LinkedLocations.Count}[/] places:");
        
        int i = 0;
        foreach (var location in currentLocation.LinkedLocations)
        {
            i++;
            Console.WriteLine($"    {i}: {location.Description}");
        }
    }
}

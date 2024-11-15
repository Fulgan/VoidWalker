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
        ShowLocationAndLinkedLocations(currentLocation);
        ShowLocationSpecificCommands(currentLocation);
    }

    private static void ShowLocationAndLinkedLocations(Location currentLocation)
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
            Console.WriteLine($"    {i}) {location.Description}");
        }
    }

    private static void ShowLocationSpecificCommands(Location currentLocation)
    {
        // TODO: polymorphism? A bunch of lil classes like "ShowInnCommand"? idk.
        if (currentLocation is Inn inn)
        {
            var innCost = inn.InnCost;
            AnsiConsole.MarkupLine($"It costs [#0000aa]{innCost} gold[/] to stay at this inn for the night. Type [#f00]S[/] to sleep.");
        }
    }
}

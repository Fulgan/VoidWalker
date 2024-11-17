using Spectre.Console;
using TextBlade.Core.IO;
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

        AnsiConsole.MarkupLine($"You are in [{Colours.Highlight}]{currentLocation.Name}: {currentLocation.Description}[/]");
        var extaDescription = currentLocation.GetExtraDescription();
        if (extaDescription != null)
        {
            AnsiConsole.MarkupLine(extaDescription);
        }

        AnsiConsole.MarkupLine($"You can go to [{Colours.Highlight}]{currentLocation.LinkedLocations.Count}[/] places:");
        
        int i = 0;
        foreach (var location in currentLocation.LinkedLocations)
        {
            i++;
            Console.WriteLine($"    {i}) {location.Description}");
        }
        var extraOption = currentLocation.GetExtraMenuOption();
        if (extraOption != null)
        {
            AnsiConsole.MarkupLine(extraOption);
        }
    }

    private static void ShowLocationSpecificCommands(Location currentLocation)
    {
        // TODO: polymorphism? A bunch of lil classes like "ShowInnCommand"? idk.
        if (currentLocation is Inn inn)
        {
            var innCost = inn.InnCost;
            AnsiConsole.MarkupLine($"It costs [{Colours.Highlight}]{innCost} gold[/] to stay at this inn for the night. Type [{Colours.Command}]S[/] to sleep.");
        }
    }
}

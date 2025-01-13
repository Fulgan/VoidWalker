using System.Globalization;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner;

public class LocationDisplayer
{
    private IConsole _console;
    
    public LocationDisplayer(IConsole console)
    {
        _console = console;
    }

    /// <summary>
    /// Show a location.
    /// </summary>
    public void ShowLocation(Location currentLocation)
    {
        ShowLocationAndLinkedLocations(currentLocation);
        ShowLocationSpecificCommands(currentLocation);
        ShowNpcsIfAny(currentLocation);
    }

    private void ShowLocationAndLinkedLocations(Location currentLocation)
    {
        if (currentLocation == null)
        {
            throw new InvalidOperationException("Current location is null!");
        }

        _console.WriteLine($"You are in [{Colours.Highlight}]{currentLocation.Name}: {currentLocation.Description}[/]");
        var extaDescription = currentLocation.GetExtraDescription();
        if (extaDescription != null)
        {
            _console.WriteLine($"[#aae]{extaDescription}[/]");
        }

        _console.WriteLine($"You can go to [{Colours.Highlight}]{currentLocation.LinkedLocations.Count}[/] places:");
        
        int i = 0;
        foreach (var location in currentLocation.LinkedLocations)
        {
            i++;
            _console.WriteLine($"    {i}) {location.Description}");
        }

        var extraOption = currentLocation.GetExtraMenuOptions();
        if (extraOption != null)
        {
            _console.WriteLine($"[{Colours.Command}]{extraOption}[/]");
        }
    }

    private void ShowLocationSpecificCommands(Location currentLocation)
    {
        // TODO: polymorphism? A bunch of lil classes like "ShowInnCommand"? idk.
        if (currentLocation is Inn inn)
        {
            var innCost = inn.InnCost;
            _console.WriteLine($"It costs [{Colours.Highlight}]{innCost} gold[/] to stay at this inn for the night. Type [{Colours.Command}]S[/] to sleep.");
        }
    }

    private void ShowNpcsIfAny(Location currentLocation)
    {
        _console.WriteLine($"You can talk to the following {currentLocation.Npcs.Length} entities:");
        foreach (var npc in currentLocation.Npcs)
        {
            _console.WriteLine($"    {npc.Name}");
        }
    }
}

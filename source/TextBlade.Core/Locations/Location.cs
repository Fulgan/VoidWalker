using TextBlade.Core.Audio;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Locations;

/// <summary>
/// Your regular location. Has reachable locations (sub-locations, adjacent locations, however you concieve of it.)
/// </summary>
public class Location
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Polymorphic? for simplicity. One audio for convencience, or an array if you like.
    public string BackgroundAudio { get; set; } = string.Empty;
    public string[] BackgroundAudios { get; set; } = [];
    
    public Npc[] Npcs { get; set; } = [];

    /// <summary>
    /// For custom code, this is the class name of the code-behind class for this location.
    /// </summary>
    public string? LocationClass { get; set; }

    public List<LocationLink> LinkedLocations { get; set; } = new();
    public string LocationId { get; set; } = null!; // Saved so we know our location; public for tests.
    
    public Location(string name, string description, string? locationClass = null)
    {
        this.Name = name;
        this.Description = description;
        this.LocationClass = locationClass;
    }

    public IEnumerable<LocationLink> VisibleLocations => LinkedLocations.Where(l => l.SwitchRequired is null || (GameSwitches.Switches.Has(l.SwitchRequired) && GameSwitches.Switches.Get(l.SwitchRequired) == true));

    public virtual ICommand GetCommandFor(string input)
    {
        // Leave it up to sub-types, like inn, to handle their own input and return a command.
        return new DoNothingCommand();
    }

    public virtual string GetExtraDescription()
    {
        // Override for stuff like "You are in 2B, you see three Tiramisu Bettles"
        return string.Empty;
    }

    public virtual string GetExtraMenuOptions()
    {
        // Override for stuff like "type f/fight to fight"
        return string.Empty;
    }

    public virtual string GetExtraOutputFor(string rawResponse)
    {
        // Override for Dungeon only. Stuff like "Can't descend, monsters around."
        return string.Empty;
    }
    
    public virtual Dictionary<string, object>? GetCustomSaveData()
    {
        return null; // nothing extra to save
    }

    // The opposite/twin of GetCustomSaveData. Sets state based on saved data.
    // Note that, if a location saves this, and a second location sets this, we overwrite the first data.
    // Meaning, go halfway to dungeon 2, then back to dungeon 1, then back to dungeon 2 and it resets.
    public virtual void SetStateBasedOnCustomSaveData(Dictionary<string, object>? extraData)
    {
    }
}


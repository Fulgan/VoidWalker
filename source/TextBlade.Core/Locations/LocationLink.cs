namespace TextBlade.Core.Locations;

/// <summary>
/// A link to another location, with a text description.
/// e.g. Id = Lake, description = "You see a lake nearby."
/// </summary>
public class LocationLink
{
    public string Id { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// If set, this location link requires this particular named switch to be true before it appears in the location list and is travelable
    /// </summary>
    public string? SwitchRequired { get; set; }

    public LocationLink(string id, string description, string? switchRequired = null)
    {
        this.Id = id;
        this.Description = description;
        this.SwitchRequired = switchRequired;
    }
}

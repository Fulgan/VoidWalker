namespace TextBlade.Core.Locations;

/// <summary>
/// A link to another location, with a text description.
/// e.g. Id = Lake, description = "You see a lake nearby."
/// </summary>
public class LocationLink
{
    public string Id { get; set; }
    public string Description { get; set; }

    public LocationLink(string id, string description)
    {
        this.Id = id;
        this.Description = description;
    }
}

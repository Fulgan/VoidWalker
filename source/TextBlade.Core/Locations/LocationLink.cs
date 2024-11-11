using System;

namespace TextBlade.Core.Locations;

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

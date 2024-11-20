namespace TextBlade.Core.Locations;

/// <summary>
/// Base class that includes all common fields across all types of locations.
/// </summary>
public abstract class BaseLocation
{
    public string Name { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// For custom code, this is the class name of the code-behind class for this location.
    /// </summary>
    public string? LocationClass { get; set; }

    protected BaseLocation(string name, string description, string? locationClass)
    {
        this.Name = name;
        this.Description = description;
        this.LocationClass = locationClass;
    }
}
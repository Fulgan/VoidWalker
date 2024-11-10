namespace TextBlade.Core.Locations;

public abstract class Region
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Region(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }
}
namespace TextBlade.Core.Locations;

/// <summary>
/// An inn: you pay the InnCost, and get healed up.
/// </summary>
public class Inn : Location
{
    public Inn(string name, string description) : base(name, description)
    {
    }

    public int InnCost { get; set; }
}


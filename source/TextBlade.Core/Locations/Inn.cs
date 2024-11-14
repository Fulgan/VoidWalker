using TextBlade.Core.Commands;

namespace TextBlade.Core.Locations;

/// <summary>
/// An inn: you pay the InnCost, and get healed up.
/// </summary>
public class Inn : Location
{
    public int InnCost { get; set; }

    public Inn(string name, string description) : base(name, description)
    {
    }

    public override Command GetCommandFor(string input)
    {
        if (input.ToLower() == "s")
        {
            return new SleepAtInnCommand(this.InnCost);
        }

        return new DoNothingCommand();
    }
}


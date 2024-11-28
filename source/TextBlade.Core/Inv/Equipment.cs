using System.Text;

namespace TextBlade.Core.Inv;

public class Equipment : Item
{
    // Supposed to be readonly. We need to serialize it though.
    public Dictionary<CharacterStats, int> StatsModifiers { get; private set; } = new();

    // This is obvious, but maybe not? Best to enshrine it in code.
    public Equipment(string name, string itemType, Dictionary<CharacterStats, int> statsModifiers) : base(name, string.Empty, itemType)
    {
        if (this.ItemType == ItemType.Consumable)
        {
            throw new ArgumentException("Equipment can't be consumable", nameof(itemType));
        }

        if (statsModifiers == null || statsModifiers.Keys.Count == 0)
        {
            throw new ArgumentException("Equipment needs stats modifiers.");
        }

        StatsModifiers = statsModifiers;
    }

    /// <summary>
    /// A convenient way to represent our stats in a human-readable/understandable way
    /// </summary>
    public override string ToString()
    {
        var stats = new StringBuilder();
        foreach (var stat in StatsModifiers)
        {
            var statSign = "";
            if (stat.Value > 0)
            {
                statSign = "+";
            }
            
            stats.AppendLine($"{stat.Key} {statSign}{stat.Value}");
        }
        return stats.ToString();
    }
}

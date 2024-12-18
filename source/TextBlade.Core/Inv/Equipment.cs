using System.Text;

namespace TextBlade.Core.Inv;

public class Equipment : Item
{
    // Supposed to be readonly. We need to serialize it though.
    public Dictionary<CharacterStats, int> StatsModifiers { get; private set; } = new();

    public string? DamageType { get; set; }

    // This is obvious, but maybe not? Best to enshrine it in code.
    public Equipment(string name, string itemType, Dictionary<CharacterStats, int> statsModifiers, string? damageType = null, int value = 1) : base(name, string.Empty, itemType, value)
    {
        if (this.ItemType == ItemType.Consumable)
        {
            throw new ArgumentException("Equipment can't be consumable", nameof(itemType));
        }

        if (statsModifiers == null || statsModifiers.Keys.Count == 0)
        {
            throw new ArgumentException("Equipment needs stats modifiers.");
        }

        if (damageType != null && damageType != "Normal" && itemType != ItemType.Weapon.ToString())
        {
            throw new ArgumentException("DamageType only applies to weapons", nameof(damageType));
        }

        StatsModifiers = statsModifiers;
        DamageType = damageType;
    }

    public int GetStatsModifier(CharacterStats stat)
    {
        if (!StatsModifiers.ContainsKey(stat))
        {
            return 0;
        }

        return StatsModifiers[stat];
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
            
            stats.Append($"{stat.Key} {statSign}{stat.Value} ");
        }
        return stats.ToString().Trim();
    }
}

namespace TextBlade.Core.Battle;

/// <summary>
/// Yer basic, data-driven skill. All fields are optional, since it covers way too many cases.
/// This is probably in need of refactoring into small, single-responsibility classes.
/// </summary>
public class Skill
{
    public string Name { get; set; } = string.Empty;
    public float DamageMultiplier { get; set; } = 1.0f;
    public int Cost { get; set; } = 0;
    // ENUM: Single, All (enemies), Self, Character, Party
    public string Target { get; set; } = "Single";
    public string StatusInflicted { get; set; } = string.Empty;
    public int StatusStacks { get; set; } = 0;
    public string DamageType { get; set; } = "Normal";

    private const double MaxPrecisionDifference = 0.00001;

    public override string ToString()
    {
        var statusText = string.IsNullOrWhiteSpace(StatusInflicted) ? string.Empty : $"inflicts {StatusInflicted} {StatusStacks} times,";
        if (Math.Abs(DamageMultiplier - 0) < MaxPrecisionDifference)
        {
            return $"{Name}: {statusText} costs {Cost} skill points";
        }

        var dealsOrHeals = DamageMultiplier > 0 ? $"Deals {DamageMultiplier}" : $"Heals {-DamageMultiplier}";
        return $"{Name}: {dealsOrHeals}x {DamageType}-type damage against {Target}, {statusText} costs {Cost} skill points";
    }
}

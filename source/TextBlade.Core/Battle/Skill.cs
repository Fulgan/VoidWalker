using System;

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
    // ENUM: Enemy, All (enemies), Self, Character, Party
    public string Target { get; set; } = "Enemy";
    public string StatusInflicted { get; set; } = string.Empty;
    public int StatusStacks { get; set; } = 0;
    public string DamageType { get; set; } = "Normal";
}

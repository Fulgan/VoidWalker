using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

/// <summary>
/// Yer basic, data-driven skill. All fields are optional, since it covers way too many cases.
/// This is probably in need of refactoring into small, single-responsibility classes.
/// </summary>
public class Skill
{
    private static IDictionary<string, Skill> s_allSkillsData;
    public static Skill GetSkill(string skillName) => s_allSkillsData[skillName];

    public string Name { get; set; } = string.Empty;
    public float DamageMultiplier { get; set; } = 1.0f;
    public int Cost { get; set; } = 0;

    // SingleEnemy, AllEnemies, SingleFriend, AllFriends.
    // Meaning changes depending on if a character or monster uses it.
    public string Target { get; set; } = "SingleEnemy";
    public string StatusInflicted { get; set; } = string.Empty;
    public int StatusStacks { get; set; } = 0;
    public string DamageType { get; set; } = "Normal";

    private const double MaxPrecisionDifference = 0.00001;

    static Skill()
    {
        s_allSkillsData = Serializer.DeserializeSkillsData();
    }

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

    /// <summary>
    /// Returns the audio file name, if the file exists.
    /// Otherwise, returns empty string.
    /// Assumes a lot of things...
    /// </summary>
    internal string GetAudioFileName()
    {
        // Assumes too much.
        var toReturn = Path.Join("Content", "Audio", "sfx", "skills", $"{Name.ToLower().Replace(' ', '-')}.wav");
        return File.Exists(toReturn) ? toReturn : string.Empty;
    }
}

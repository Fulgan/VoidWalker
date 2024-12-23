using System;

namespace TextBlade.Core.Battle;

public class Spoils
{
    public int GoldGained { get; set; }
    public int ExperiencePointsGained { get; set; }
    public List<string> Loot { get; set; } = new();
    public bool IsVictory { get; set; }
}

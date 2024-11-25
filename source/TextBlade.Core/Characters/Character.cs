using TextBlade.Core.Battle;

namespace TextBlade.Core.Characters;

public class Character
{
    public string Name { get; set; } = string.Empty;
    public int CurrentHealth { get; set; }
    public int TotalHealth { get; set; }
    public int CurrentSkillPoints { get; set; }
    public int TotalSkillPoints { get; set; }
    public int Strength { get; set; }
    public int Toughness { get; set; }
    public List<Skill> Skills { get; set; } = new(); // NOT populated by JSON
    public List<string> SkillNames { get; set; } = new(); // populated by JSON
    internal bool IsDefending { get; private set; }

    public void OnRoundComplete()
    {
        this.IsDefending = false;
    }

    // Attack/damage logic is centralized in the Monster class

    internal void Defend()
    {
        this.IsDefending = true;
    }
}

using TextBlade.Core.Battle;

namespace TextBlade.Core.Characters;

public class Character : Entity
{
    public int TotalSkillPoints { get; set; }
    public int CurrentSkillPoints { get; set; }
    public List<Skill> Skills { get; set; } = new(); // NOT populated by JSON
    public List<string> SkillNames { get; set; } = new(); // populated by JSON

    // Used for skills.
    public readonly int Special;
    public readonly int SpecialDefense;

    internal bool IsDefending { get; private set; }

    public Character(string name, int health, int strength, int toughness, int special = 0, int specialDefense = 0)
    : base(name, health, strength, toughness)
    {
        this.Special = special;
        this.SpecialDefense = specialDefense;
    }

    public void FullyHeal()
    {
        this.CurrentHealth = this.TotalHealth;
        this.CurrentSkillPoints = this.TotalSkillPoints;
    }

    public void Revive()
    {
        this.CurrentHealth = 1;
    }

    new internal List<string> OnRoundComplete()
    {
        this.IsDefending = false;
        return base.OnRoundComplete();
    }

    internal void Defend()
    {
        this.IsDefending = true;
    }
}

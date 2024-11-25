using TextBlade.Core.Battle;

namespace TextBlade.Core.Characters;

public class Character : Entity
{
    public int TotalSkillPoints { get; set; }
    public int CurrentSkillPoints { get; set; }
    public List<Skill> Skills { get; set; } = new(); // NOT populated by JSON
    public List<string> SkillNames { get; set; } = new(); // populated by JSON

    internal bool IsDefending { get; private set; }

    public Character(string name, int health, int strength, int toughness)
    : base(name, health, strength, toughness)
    {
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

    new internal void OnRoundComplete()
    {
        this.IsDefending = false;
        base.OnRoundComplete();
    }

    internal void Defend()
    {
        this.IsDefending = true;
    }
}

using TextBlade.Core.Battle;

namespace TextBlade.Core.Characters;

public class Monster : Entity
{
    public readonly string Weakness;
    public readonly int Gold;
    public readonly int ExperiencePoints;

    public Dictionary<string, double>? SkillProbabilities { get; }

    public Monster(string name, int health, int strength, int toughness, int gold = 0, int experiencePoints = 0, string weakness = "", List<Skill>? skills = null, Dictionary<string, double>? stringProbabilities = null)
    : base(name, health, strength, toughness)
    {
        this.Weakness = weakness;
        this.Gold = gold;

        this.ExperiencePoints = experiencePoints;
        if (this.ExperiencePoints == 0)
        {
            // No legitimate case for this right now; determine it automagically.
            this.ExperiencePoints = strength + toughness;
        }

        this.Skills = skills;
        this.SkillProbabilities = stringProbabilities;
    }

    internal int Attack(Character target)
    {
        var attack = this.Strength;
        var blocked = target.TotalToughness;
        if (target.IsDefending)
        {
            blocked = (int)(blocked * 1.5);
        }
        
        var damage = Math.Max(0, attack - blocked);
        target.Damage(damage);

        return damage;
    }

    public override string ToString()
    {
        return $"{this.Name} ({this.CurrentHealth}/{this.TotalHealth} health)";
    }
}

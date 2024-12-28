using TextBlade.Core.Battle;

namespace TextBlade.Core.Characters;

public class Monster : Entity
{
    public List<string>? SkillNames { get; set; } = new(); // populated by JSON

    public readonly string Weakness;
    public readonly int Gold;
    public readonly int ExperiencePoints;

    public Dictionary<string, double>? SkillProbabilities { get; }

    public Monster(string name, int health, int strength, int toughness, int special, int specialDefense, int skillPoints, int experiencePoints, int gold = 0, string weakness = "", List<Skill>? skills = null, Dictionary<string, double>? stringProbabilities = null)
    : base(name, health, strength, toughness, special, specialDefense, skillPoints)
    {
        this.Weakness = weakness;
        this.Gold = gold;
        this.ExperiencePoints = experiencePoints;
        this.Skills = skills ?? new();
        this.SkillProbabilities = stringProbabilities ?? new();
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
        var skillPointsMessage = this.TotalSkillPoints > 0 ? $", {this.CurrentSkillPoints}/{this.TotalSkillPoints} skill points" : "";
        return $"{this.Name} ({this.CurrentHealth}/{this.TotalHealth} health{skillPointsMessage})";
    }
}

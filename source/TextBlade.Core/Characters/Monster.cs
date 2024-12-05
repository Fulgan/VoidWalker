namespace TextBlade.Core.Characters;

public class Monster : Entity
{
    public readonly string Weakness;
    public readonly int Gold;
    public readonly int ExperiencePoints;
    
    public Monster(string name, int health, int strength, int toughness, int gold = 0, int experiencePoints = 0, string weakness = "")
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
}

namespace TextBlade.Core.Characters;

public class Monster : Entity
{
    public string Weakness { get; private set; } = string.Empty;
    
    public Monster(string name, int health, int strength, int toughness, string weakness = "")
    : base(name, health, strength, toughness)
    {
        this.Weakness = weakness;
    }

    internal int Attack(Character target)
    {
        var attack = this.Strength;
        var blocked = target.Toughness;
        if (target.IsDefending)
        {
            blocked = (int)(blocked * 1.5);
        }
        
        var damage = Math.Max(0, attack - blocked);
        target.Damage(damage);

        return damage;
    }
}

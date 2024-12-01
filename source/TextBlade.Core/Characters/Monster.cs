namespace TextBlade.Core.Characters;

public class Monster : Entity
{
    public readonly string Weakness;
    public readonly int Gold;
    
    public Monster(string name, int health, int strength, int toughness, int gold = 0, string weakness = "")
    : base(name, health, strength, toughness)
    {
        this.Weakness = weakness;
        this.Gold = gold;
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

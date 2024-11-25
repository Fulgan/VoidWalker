namespace TextBlade.Core.Characters;

public class Monster
{
    public string Name { get; set; }
    public int TotalHealth { get; set; }
    public int CurrentHealth { get; private set; } 
    public int Strength { get; private set; } 
    public int Toughness { get; private set; }
    public string Weakness { get; private set; } = string.Empty;
    public Dictionary<string, int> StatusStacks { get; private set; } = new();
    
    public Monster(string name, int health, int strength, int toughness, string weakness = "")
    {
        this.Name = name;
        this.CurrentHealth = health;
        this.TotalHealth = health;
        this.Strength = strength;
        this.Toughness = toughness;
        this.Weakness = weakness;
    }

    public void Damage(int amount)
    {
        CurrentHealth = Math.Max(0, CurrentHealth - amount);
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
        target.CurrentHealth -= damage;
        
        if (target.CurrentHealth < 0)
        {
            target.CurrentHealth = 0;
        }

        return damage;
    }

    internal void InflictStatus(string status, int stacks)
    {
        if (!StatusStacks.ContainsKey(status))
        {
            StatusStacks[status] = 0;
        }

        StatusStacks[status] += stacks;
    }
}

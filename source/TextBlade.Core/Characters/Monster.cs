namespace TextBlade.Core.Characters;

public class Monster
{
    public string Name { get; set; }
    public int TotalHealth { get; set; }
    public int CurrentHealth { get; private set; } 
    public int Strength { get; private set; } 
    public int Toughness { get; private set; }
    
    public Monster(string name, int health, int strength, int toughness)
    {
        this.Name = name;
        this.CurrentHealth = health;
        this.TotalHealth = health;
        this.Strength = strength;
        this.Toughness = toughness;
    }

    public void Damage(int amount)
    {
        CurrentHealth = Math.Max(0, CurrentHealth - amount);
    }
}

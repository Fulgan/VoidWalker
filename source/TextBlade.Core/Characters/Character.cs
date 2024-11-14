using System;

namespace TextBlade.Core.Characters;

public class Character
{
    public string Name { get; set; }
    public int CurrentHealth { get; set; }
    public int TotalHealth { get; set; }

    public Character(string name, int currentHealth, int totalHealth)
    {
        this.Name = name;
        this.CurrentHealth = currentHealth;
        this.TotalHealth = totalHealth;
    }
}

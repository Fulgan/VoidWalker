using System;

namespace TextBlade.Core.Characters;

public class Monster
{
    public string MonsterName { get; set; }
    public int TotalHealth { get; set; }
    public int CurrentHealth { get; private set; } 
    public int Strength { get; private set; } 
    public int Defense { get; private set; }
    
    public Monster(int health, int strength, int defense)
    {
        this.CurrentHealth = health;
        this.TotalHealth = health;
        this.Strength = strength;
        this.Defense = defense;
    }
}

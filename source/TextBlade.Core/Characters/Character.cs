using System;

namespace TextBlade.Core.Characters;

public class Character
{
    public string Name { get; set; }
    public int CurrentHealth { get; set; }
    public int TotalHealth { get; set; }
    public int Strength { get; set; }
    public int Toughness { get; set; }
    private bool _isDefending;

    public Character(string name, int currentHealth, int totalHealth)
    {
        this.Name = name;
        this.CurrentHealth = currentHealth;
        this.TotalHealth = totalHealth;
    }

    public void OnRoundComplete()
    {
        _isDefending = false;
    }

    internal void Defend()
    {
        _isDefending = true;
    }
}

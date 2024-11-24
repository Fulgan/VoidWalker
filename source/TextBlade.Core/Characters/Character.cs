using TextBlade.Core.Battle;

namespace TextBlade.Core.Characters;

public class Character
{
    public string Name { get; set; }
    public int CurrentHealth { get; set; }
    public int TotalHealth { get; set; }
    public int Strength { get; set; }
    public int Toughness { get; set; }
    public List<Skill> Skills { get; set; } = new(); // NOT populated by JSON
    public List<string> SkillNames { get; set; } = new(); // populated by JSON
    internal bool IsDefending { get; private set; }


    public Character(string name, int currentHealth, int totalHealth)
    {
        this.Name = name;
        this.CurrentHealth = currentHealth;
        this.TotalHealth = totalHealth;
    }

    public void OnRoundComplete()
    {
        this.IsDefending = false;
    }

    internal void Defend()
    {
        this.IsDefending = true;
    }
}

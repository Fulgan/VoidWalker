using TextBlade.Core.Battle;
using TextBlade.Core.Characters.PartyManagement;
using TextBlade.Core.Inv;

namespace TextBlade.Core.Characters;

public class Character : Entity
{
    public int TotalSkillPoints { get; set; }
    public int CurrentSkillPoints { get; set; }
    public List<Skill> Skills { get; set; } = new(); // NOT populated by JSON
    public List<string> SkillNames { get; set; } = new(); // populated by JSON
    public Dictionary<ItemType, Equipment> Equipment { get; set; } = new(); // Needs to be public for serialization
    public int ExperiencePoints { get; internal set; } = 0;
    public int Level { get; set; } = 1;

    public int Special { get; internal set; } 
    public int SpecialDefense { get; internal set; }

    internal bool IsDefending { get; private set; }

    public Character(string name, int health, int strength, int toughness, int special = 0, int specialDefense = 0, int level = 1, int experiencePoints = 0)
    : base(name, health, strength, toughness)
    {
        this.Special = special;
        this.SpecialDefense = specialDefense;
        this.Level = level;
        this.ExperiencePoints = experiencePoints;
    }

    public void FullyHeal()
    {
        this.CurrentHealth = this.TotalHealth;
        this.CurrentSkillPoints = this.TotalSkillPoints;
    }

    public void Revive()
    {
        this.CurrentHealth = 1;
    }

    internal void GetExperiencePoints(int experiencePoints)
    {
        if (this.CurrentHealth <= 0)
        {
            return;
        }

        this.ExperiencePoints += experiencePoints;
        if (LevelManager.CanLevelUp(this))
        {
            LevelManager.LevelUp(this);
        }
    }

    internal Equipment? EquippedOn(ItemType slot)
    {
        Equipment? currentlyEquipped;
        if (Equipment.TryGetValue(slot, out currentlyEquipped))
        {
            return currentlyEquipped;
        }
        
        return null; // nothing equipped
    }

    new internal List<string> OnRoundComplete()
    {
        this.IsDefending = false;
        return base.OnRoundComplete();
    }

    internal void Defend()
    {
        this.IsDefending = true;
    }    
}

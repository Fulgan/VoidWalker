namespace TextBlade.Core.Inv;

public class Consumable : Item
{
    public int RestoreHealth { get; private set; }
    public int RestoreSkillPoints { get; private set; }

    // TODO: store JSON so we can see custom attributes
    public Consumable(string name, string description, string itemType, int restoreHealth, int restoreSkillPoints, int value = 1) : base(name, description, itemType, value)
    {
        if (this.ItemType != ItemType.Consumable)
        {
            throw new ArgumentException("Consumables must have ItemType of Consumable!", nameof(itemType));
        }

        if (restoreHealth < 0)
        {
            throw new ArgumentException("Health restore can't be negative.", nameof(restoreHealth));
        }

        if (restoreSkillPoints < 0)
        {
            throw new ArgumentException("Skill Points restore can't be negative.", nameof(restoreSkillPoints));
        }

        if (restoreHealth == 0 && restoreSkillPoints == 0)
        {
            // There's no reason to have both as zero as far as the current game
            throw new ArgumentException("Consumable doesn't do anything...");
        }

        this.RestoreHealth = restoreHealth;
        this.RestoreSkillPoints = restoreSkillPoints;
    }
}

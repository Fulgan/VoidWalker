namespace TextBlade.Core.Characters;

/// <summary>
/// The common ancestor of Monster and Character. Contains common logic and attributes.
/// </summary>
public abstract class Entity
{
    
    public string Name { get; protected set; }
    public int TotalHealth { get; protected set; }
    public int CurrentHealth { get; protected set; } 
    public int Strength { get; protected set; } 
    public int Toughness { get; protected set; }
    public Dictionary<string, int> StatusStacks { get; private set; } = new();

    protected Entity(string name, int health, int strength, int toughness)
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

    public void OnRoundComplete()
    {
        // Apply statuses
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

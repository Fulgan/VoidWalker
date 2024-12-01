using TextBlade.Core.Battle.Statuses;

namespace TextBlade.Core.Characters;

/// <summary>
/// The common ancestor of Monster and Character. Contains common logic and attributes.
/// </summary>
public abstract class Entity
{
    public string Name { get; protected set; }
    public int TotalHealth { get; set; }
    public int CurrentHealth { get; set; } 
    public int Strength { get; internal set; } 
    public int Toughness { get; internal set; }
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

    public List<string> OnRoundComplete()
    {
        return ApplyStatuses();
    }

    internal void InflictStatus(string status, int stacks)
    {
        if (!StatusStacks.ContainsKey(status))
        {
            StatusStacks[status] = 0;
        }

        StatusStacks[status] += stacks;
    }

    private List<string> ApplyStatuses()
    {
        var toReturn = new List<string>();
        var finishedStatuses = new List<string>();

        foreach (var kvp in StatusStacks)
        {
            var statusName = kvp.Key;
            // TODO: refactor, this stinks. Maybe post-prototype?
            switch (statusName.ToLowerInvariant())
            {
                case "poison":
                    toReturn.Add(Poisoner.Poison(this));
                    break;
                case "burn":
                    toReturn.Add(Burner.Burn(this));
                    break;
                default:
                    throw new InvalidOperationException($"Missing implementation for the status effect {statusName}");
            }
            
            var stacksLeft = kvp.Value;
            stacksLeft--;

            if (stacksLeft <= 0)
            {
                finishedStatuses.Add(kvp.Key);
            }
        }

        // Remove finished/done statuses
        foreach (var key in finishedStatuses)
        {
            StatusStacks.Remove(key);
        }

        return toReturn;
    }
}

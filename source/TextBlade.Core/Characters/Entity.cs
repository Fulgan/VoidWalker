using Newtonsoft.Json;
using TextBlade.Core.Battle;
using TextBlade.Core.Battle.Statuses;
using TextBlade.Core.IO;

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
    public int Special { get; internal set; } 
    public int SpecialDefense { get; internal set; }
    public int TotalSkillPoints { get; set; }
    public int CurrentSkillPoints { get; set; }

    public Dictionary<string, int> StatusStacks { get; private set; } = new();
    
    [JsonIgnore]
    public List<Skill> Skills { get; set; } = new(); // NOT populated by JSON
    
    protected Entity(string name, int health, int strength, int toughness, int special, int specialDefense, int skillPoints)
    {
        this.Name = name;
        this.CurrentHealth = health;
        this.TotalHealth = health;
        this.Strength = strength;
        this.Toughness = toughness;
        this.Special = special;
        this.SpecialDefense = specialDefense;
        this.CurrentSkillPoints = skillPoints;
        this.TotalSkillPoints = skillPoints;
    }

    public bool IsAlive => CurrentHealth > 0;

    public void Damage(int amount)
    {
        CurrentHealth = Math.Max(0, CurrentHealth - amount);
    }

    public virtual void OnRoundComplete(IConsole console)
    {
        ApplyStatuses(console);
    }

    internal void InflictStatus(string status, int stacks)
    {
        if (!StatusStacks.ContainsKey(status))
        {
            StatusStacks[status] = 0;
        }

        StatusStacks[status] += stacks;
    }

    private void ApplyStatuses(IConsole console)
    {
        var finishedStatuses = new List<string>();

        foreach (var kvp in StatusStacks)
        {
            var statusName = kvp.Key;
            // TODO: refactor, this stinks. Maybe post-prototype?
            switch (statusName.ToLowerInvariant())
            {
                case "poison":
                    new Poisoner(console).Poison(this);
                    break;
                case "burn":
                    new Burner(console).Burn(this);
                    break;
                case "paralyze":
                    break; // Implemented in turn-based system
                default:
                    throw new InvalidOperationException($"There's no implementation for the status effect {statusName}");
            }
            
            StatusStacks[statusName] = StatusStacks[statusName] - 1;

            if (StatusStacks[statusName] <= 0)
            {
                finishedStatuses.Add(kvp.Key);
            }
        }

        // Remove finished/done statuses
        foreach (var key in finishedStatuses)
        {
            StatusStacks.Remove(key);
        }
    }
}

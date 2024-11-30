using System.Text;
using TextBlade.Core.Inv;

namespace TextBlade.Core.Characters.PartyManagement;

public static class EquipmentDiffer
{
    public static Dictionary<CharacterStats, int> GetDiff(Equipment? current, Equipment? newThing)
    {
        if (current == null && newThing == null)
        {
            return new Dictionary<CharacterStats, int>();
        }
        
        // Easy peasy lemon squeezy cases
        // 1) Equip when we have nothing: new stats take precedence
        if (current == null && newThing != null)
        {
            return newThing.StatsModifiers;
        }

        // 2) Unequip: negate current stats
        if (current != null && newThing == null)
        {
            var toReturn = new Dictionary<CharacterStats, int>();
            foreach (var key in current.StatsModifiers.Keys)
            {
                toReturn.Add(key, -current.StatsModifiers[key]);
            }
            return toReturn;
        }

        // Three cases:
        // 1) Present in new, not in current
        // 2) Present in current, not in new
        // 3) Present in both
        var currentStats = current.StatsModifiers;
        var newStats = newThing.StatsModifiers;
        var diff = new Dictionary<CharacterStats, int>();

        // Case 3: present in both
        var commonStats = currentStats.Keys.Intersect(newStats.Keys);
        foreach (var stat in commonStats)
        {
            diff[stat] = newStats[stat] - currentStats[stat];
        }

        // Case 1: present in new. They're additive.
        var inNewOnly = newStats.Keys.Where(k => !commonStats.Contains(k));
        foreach (var stat in inNewOnly)
        {
            diff[stat] = newStats[stat];
        }

        // Case 2: present in old. They're not in new, so they're negated.
        var currentOnly = currentStats.Keys.Where(c => !commonStats.Contains(c));
        foreach (var stat in currentOnly)
        {
            diff[stat] = -currentStats[stat];
        }

        return diff;
    }

    public static string DiffToString(Dictionary<CharacterStats, int> diff)
    {
        var toReturn = new StringBuilder();
        foreach (var kvp in diff)
        {
            var value = kvp.Value <= 0 ? kvp.Value.ToString() : $"+{kvp.Value}"; 
            toReturn.AppendLine($"{kvp.Key} {value}");
        }
        return toReturn.ToString();
    }
}

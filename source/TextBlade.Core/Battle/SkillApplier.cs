using System.Text;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Battle;

public static class SkillApplier
{
    internal static string Apply(Character user, Skill skill, IEnumerable<Entity> targets)
    {
        var message = new StringBuilder();
        
        foreach (var target in targets)
        {
            message.Append(ApplyDamage(user, skill, target));
            message.AppendLine(InflictStatuses(user, skill, target));
        }

        user.CurrentSkillPoints -= skill.Cost;
        return message.ToString();
    }

    private static string ApplyDamage(Character user, Skill skill, Entity target)
    {
        /////// TODO: REFACTOR so this method is not polymorphic
        ArgumentNullException.ThrowIfNull(target);
        float damage = 0;
        var hitWeakness = false;

        if (target is Monster m)
        {
            damage = (user.Strength - target.Toughness) * skill.DamageMultiplier;
            if (m.Weakness == skill.DamageType)
            {
                // Targeting their weakness? 2x damage!
                damage *= 2;
                hitWeakness = true;
            }
        }
        else if (target is Character)
        {
            // If you're healing, heal for 2x
            damage = (int)Math.Ceiling(user.Special * skill.DamageMultiplier * 2);
        }

        var roundedDamage = (int)damage;
        target.Damage(roundedDamage);
        // TODO: DRY the 2x damage part with CharacterTurnProcessor
        var damageMessage = damage > 0 ? $"{roundedDamage} damage" : $"healed for [green]{-roundedDamage}[/]";
        var effectiveMessage = hitWeakness ? "[#f80]Super effective![/]" : "";
        return $"{user.Name} uses {skill.Name} on {target.Name}! {effectiveMessage} {damageMessage}!";
    }
    
    private static string InflictStatuses(Character user, Skill skill, Entity target)
    {
        if (string.IsNullOrWhiteSpace(skill.StatusInflicted))
        {
            return string.Empty;
        }

        var status = skill.StatusInflicted;
        var stacks = skill.StatusStacks;
        target.InflictStatus(status, stacks);

        return $"{user.Name} inflicts {skill.StatusInflicted} x{skill.StatusStacks} on {target.Name}!";
    }
}

using System.Text;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Battle;

public static class SkillApplier
{
    internal static string Apply(Character user, Skill skill, Monster target)
    {
        var message = new StringBuilder();
        message.Append(ApplyDamage(user, skill, target));
        message.Append(InflictStatuses(user, skill, target));
        user.CurrentSkillPoints -= skill.Cost;
        return message.ToString();
    }

    private static string ApplyDamage(Character user, Skill skill, Monster target)
    {
        float damage = (user.Strength - target.Toughness) * skill.DamageMultiplier;
        if (target.Weakness == skill.DamageType)
        {
            damage *= 2;
        }

        var roundedDamage = (int)damage;
        target.Damage(roundedDamage);

        return $"{user.Name} uses {skill.Name} on {target.Name}! {roundedDamage} damage!";
    }
    
    private static string InflictStatuses(Character user, Skill skill, Monster target)
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

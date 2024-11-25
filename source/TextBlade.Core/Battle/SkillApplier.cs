using TextBlade.Core.Characters;

namespace TextBlade.Core.Battle;

public static class SkillApplier
{
    internal static void Apply(Character user, Skill skill, Monster target)
    {
        ApplyDamage(user, skill, target);
        InflictStatuses(user, skill, target);
        user.CurrentSkillPoints -= skill.Cost;
    }

    private static void ApplyDamage(Character user, Skill skill, Monster target)
    {
        float damage = (user.Strength - target.Toughness) * skill.DamageMultiplier;
        if (target.Weakness == skill.DamageType)
        {
            damage *= 2;
        }

        var roundedDamage = (int)damage;
        target.Damage(roundedDamage);

        Console.WriteLine($"{user.Name} uses {skill.Name} on {target.Name}! {roundedDamage} damage!");
    }
    
    private static void InflictStatuses(Character user, Skill skill, Monster target)
    {
        if (string.IsNullOrWhiteSpace(skill.StatusInflicted))
        {
            return;
        }

        var status = skill.StatusInflicted;
        var stacks = skill.StatusStacks;
        target.InflictStatus(status, stacks);

        Console.WriteLine($"{user.Name} inflicts {skill.StatusInflicted} x{skill.StatusStacks} on {target.Name}!");
    }
}

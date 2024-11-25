using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle.Statuses;

public static class Poisoner
{
    private const float PosionPercentDamage = 0.1f;

    public static string Poison(Entity e)
    {
        var poisonDamage = (int)(e.TotalHealth * PosionPercentDamage);
        e.Damage(poisonDamage);
        return $"{e.Name} is poisoned for [{Colours.Poison}]{poisonDamage}[/] damage!";
    }
}

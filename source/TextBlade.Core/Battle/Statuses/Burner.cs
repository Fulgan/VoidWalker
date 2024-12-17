using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle.Statuses;

public static class Burner
{
    private const float MinimumBurnDamagePercent = 0.05f;
    private const float MaximumBurnDamagePercent = 0.20f;
    private const int MinimumFireDamage = 10;

    /// <summary>
    /// Damages you for a percentage of your current health, e.g. 5-20%.
    /// Very unpredictable, like fire itself.
    /// </summary>
    public static string Burn(Entity e)
    {
        var fireSpreadPercent = MaximumBurnDamagePercent - MinimumBurnDamagePercent;
        var firePercentDamage = MinimumBurnDamagePercent + (Random.Shared.NextSingle() * fireSpreadPercent);
        
        var fireDamage = (int)Math.Max(MinimumFireDamage, firePercentDamage * e.CurrentHealth);

        e.Damage(fireDamage);
        return $"{e.Name} burns [{Colours.Fire}]{fireDamage}[/] damage!";
    }
}

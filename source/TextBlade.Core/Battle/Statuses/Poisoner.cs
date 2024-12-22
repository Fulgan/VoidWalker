using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle.Statuses;

public class Poisoner
{

    private const float PosionPercentDamage = 0.1f;
    private readonly IConsole _console;

    public Poisoner(IConsole console)
    {
        _console = console;
    }

    /// <summary>
    /// Damages you for a percent of your max health, e.g. 10%
    /// </summary>
    public void Poison(Entity e)
    {
        var poisonDamage = (int)(e.TotalHealth * PosionPercentDamage);
        e.Damage(poisonDamage);
        _console.WriteLine($"{e.Name} is poisoned for [{Colours.Poison}]{poisonDamage}[/] damage!");
    }
}

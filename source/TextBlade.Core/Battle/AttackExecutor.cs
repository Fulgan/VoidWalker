using System.Text;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

internal class AttackExecutor
{
    private readonly IConsole _console;

    public AttackExecutor(IConsole console)
    {
        _console = console;
    }

    internal void Attack(Character character, Monster targetMonster)
    {
        ArgumentNullException.ThrowIfNull(character);
        ArgumentNullException.ThrowIfNull(targetMonster);

        // Assume target number is legit
        var message = new StringBuilder();
        message.Append($"{character.Name} attacks {targetMonster.Name}! ");
        
        var damage = character.TotalStrength - targetMonster.Toughness;
        
        var characterWeapon = character.EquippedOn(Inv.ItemType.Weapon);
        // TODO: DRY with SkillApplier
        var effectiveMessage = "";
        if (characterWeapon?.DamageType == targetMonster.Weakness)
        {
            effectiveMessage = "[#f80]Super effective![/]";

            damage *= 2;
        }

        targetMonster.Damage(damage);
        
        var damageAmount = damage <= 0 ? "NO" : damage.ToString();
        message.Append($"[{Colours.Highlight}]{damageAmount}[/] damage! {effectiveMessage}");
        if (targetMonster.CurrentHealth <= 0)
        {
            message.Append($"{targetMonster.Name} DIES!");
        }
        
        _console.WriteLine(message.ToString());
    }
}

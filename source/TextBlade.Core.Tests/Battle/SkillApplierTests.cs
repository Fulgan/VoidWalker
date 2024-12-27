using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Battle;

[TestFixture]
public class SkillApplierTests
{
    [Test]
    public void Apply_AppliesDamage_IfTargetIsMonster()
    {
        // Arrange
        var applier = new SkillApplier(Substitute.For<IConsole>());
        // Skills use Strength right now (total strength), not Special
        var user = new Character("Skill User", 1, 20, 1, 1, 0, 0);
        var skill = new Skill() { DamageMultiplier = 1.5f };
        var target = new Monster("Slime", 100, 1, 5, 0, 0, 0, 0);
        int expectedDamage = (int)((user.TotalStrength - target.Toughness) * skill.DamageMultiplier);

        // Act
        applier.Apply(user, skill, [target]);

        // Assert
        var actualDamage = target.TotalHealth - target.CurrentHealth;
        Assert.That(actualDamage, Is.EqualTo(expectedDamage));
    }

    [Test]
    public void Apply_Heals_IfTargetIsCharacter()
    {
        // Arrange
        var applier = new SkillApplier(Substitute.For<IConsole>());
        // Healing uses Special
        var user = new Character("Skill User", 1, 1, 1, 20, 0, 0, 0);
        var skill = new Skill() { DamageMultiplier = 1.5f };
        var target = new Character("Ex-Slime", 100, 1, 5, 0, 0, 0) { CurrentHealth = 0 };
        int expectedHealing = (int)(user.Special * skill.DamageMultiplier * 2);

        // Act
        applier.Apply(user, skill, [target]);

        // Assert
        var actualHealing = target.CurrentHealth;
        Assert.That(actualHealing, Is.EqualTo(expectedHealing));
    }

    [Test]
    public void Apply_DoublesDamage_IfTargetWeaknessMatchesSkillDamageType()
    {
        // Arrange
        var applier = new SkillApplier(Substitute.For<IConsole>());
        var user = new Character("Skill User", 1, 20, 1, 1, 0, 0);
        var skill = new Skill() { DamageMultiplier = 1f, DamageType = "Time" };
        var target = new Monster("Slime", 100, 1, 5, 0, 0, 0, 0, weakness: "Time");
        int expectedDamage = (user.TotalStrength - target.Toughness) * 2; // x2 = weakness

        // Act
        applier.Apply(user, skill, [target]);

        // Assert
        var actualDamage = target.TotalHealth - target.CurrentHealth;
        Assert.That(actualDamage, Is.EqualTo(expectedDamage));
    }

    [Test]
    public void Apply_InflictsStatus()
    {
        // Arrange
        var applier = new SkillApplier(Substitute.For<IConsole>());
        // Skills use Strength right now (total strength), not Special
        var user = new Character("Skill User", 1, 1, 1, 1, 1, 1);
        var skill = new Skill() { StatusInflicted = "Anxiety", StatusStacks = 4 };
        var target = new Monster("Slime", 1, 1, 1, 1, 1, 1, 1);

        // Act
        applier.Apply(user, skill, [target]);

        // Assert
        Assert.That(target.StatusStacks, Does.ContainKey(skill.StatusInflicted));
        Assert.That(target.StatusStacks[skill.StatusInflicted], Is.EqualTo(skill.StatusStacks));
    }
    
    [Test]
    public void Apply_DeductsSkillPointsCost()
    {
        // Arrange
        var applier = new SkillApplier(Substitute.For<IConsole>());
        // Skills use Strength right now (total strength), not Special
        var user = new Character("Skill User", 1, 1, 1, 1, 1, 1) { TotalSkillPoints = 77, CurrentSkillPoints = 66 };
        var skill = new Skill() { Cost = 7 };
        var target = new Monster("Slime", 1, 1, 1, 1, 1, 1, 1);

        // Act
        applier.Apply(user, skill, [target]);

        // Assert
        Assert.That(user.CurrentSkillPoints, Is.EqualTo(66 - skill.Cost));
    }
}

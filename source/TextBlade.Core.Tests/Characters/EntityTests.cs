using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Characters;

[TestFixture]
public class EntityTests
{
    [Test]
    public void Damage_DoesDamage()
    {
        // Arrange
        var dummy = new EntityStub();
        
        // Act
        dummy.Damage(33);

        // Assert
        Assert.That(dummy.CurrentHealth, Is.EqualTo(dummy.TotalHealth - 33));
    }

    [Test]
    public void Damage_DoesDamage_UpToZeroHp()
    {
        // Arrange
        var dummy = new EntityStub();
        
        // Act
        dummy.Damage(9999);

        // Assert
        Assert.That(dummy.CurrentHealth, Is.EqualTo(0));
    }

    [Test]
    [TestCase("poison")]
    [TestCase("burn")]
    public void OnRoundComplete_AppliesStatuses(string status)
    {
        // Arrange
        // Let's do burn, it does damage. Easy peasy.
        var victim = new EntityStub();
        victim.StatusStacks[status] = 999;

        // Act
        victim.OnRoundComplete(Substitute.For<IConsole>());

        // Assert
        Assert.That(victim.CurrentHealth, Is.LessThan(victim.TotalHealth));
    }

    [Test]
    public void InflictStatus_AddsToStacks()
    {
        // Arrange
        var astronaut = new EntityStub();
        
        // Act
        astronaut.InflictStatus("Stress", 5);
        astronaut.InflictStatus("Radiation Poisoning", 1);
        astronaut.InflictStatus("Radiation Poisoning", 7);

        // Assert
        Assert.That(astronaut.StatusStacks["Stress"], Is.EqualTo(5));
        Assert.That(astronaut.StatusStacks["Radiation Poisoning"], Is.EqualTo(1 + 7));
    }
    

    class EntityStub : Entity
    {
        public EntityStub() : base("Practice Dummy", 100, 5, 3)
        {
        }
    }
}

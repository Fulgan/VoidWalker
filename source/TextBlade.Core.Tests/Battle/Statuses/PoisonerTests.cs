using System.Text.RegularExpressions;
using NUnit.Framework;
using TextBlade.Core.Battle.Statuses;
using TextBlade.Core.Characters;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Battle.Statuses;

[TestFixture]
public class PoisonerTests
{
    [Test]
    public void Poison_CausesDamageUpToTotalHealth()
    {
        // Arrange
        var e = new Monster("TestMon", 1000, 1, 1, 1, 1, 1, 1) { CurrentHealth = 3 };
        var console = new ConsoleStub();

        // Act
        new Poisoner(console).Poison(e);

        // Assert
        var message = console.LastMessage;
        var actual = int.Parse(Regex.Match(message, @"\](\d+)\[").Groups[1].Value);
        Assert.That(actual, Is.GreaterThanOrEqualTo(e.CurrentHealth));
        Assert.That(actual, Is.LessThan(e.TotalHealth));
        Assert.That(e.CurrentHealth, Is.EqualTo(0));
    }
}

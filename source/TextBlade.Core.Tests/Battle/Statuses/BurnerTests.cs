using System.Text.RegularExpressions;
using NUnit.Framework;
using TextBlade.Core.Battle.Statuses;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Tests.Battle.Statuses;

[TestFixture]
public class BurnerTests
{
    [Test]
    public void Burn_CausesDamageUpToCurrentHealth()
    {
        // Arrange
        var e = new Monster("TestMon", 1000, 1, 1);

        // Act
        var message = Burner.Burn(e);

        // Assert
        var actual = int.Parse(Regex.Match(message, @"\](\d+)\[").Groups[1].Value);
        Assert.That(actual, Is.LessThan(e.TotalHealth));
        Assert.That(e.CurrentHealth, Is.EqualTo(e.TotalHealth - actual));
    }
}
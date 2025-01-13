using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class SleepAtInnCommandTests
{
    [Test]
    public void Execute_ReturnsFalse_IfYouCantAffordIt()
    {
        // Arrange
        var protagonist = new Character("Cecil", 100, 50, 30, 0, 0, 0);
        protagonist.CurrentHealth = 1;
        var cost = 100;
        var command = new SleepAtInnCommand(cost);

        var saveData = new SaveData()
        {
            Party = [protagonist],
            Gold = cost / 2
        };

        // Act
        var actual = command.Execute(Substitute.For<IConsole>(), null, saveData);

        // Assert
        Assert.That(actual, Is.False);
        Assert.That(protagonist.CurrentHealth, Is.EqualTo(1)); // no heal for you!
    }

    [Test]
    public void Execute_ReturnsTrueAndHealsParty_IfYouCanAffordIt()
    {
        // Arrange
        var protagonist = new Character("Cecil", 100, 50, 30, 0, 0, 0);
        protagonist.CurrentHealth = 1;
        var duotagonist = new Character("Kain", 50, 100, 100, 0, 0, 0);
        duotagonist.CurrentHealth = 0;

        var cost = 100;
        var command = new SleepAtInnCommand(cost);

        var saveData = new SaveData()
        {
            Party = [protagonist, duotagonist],
            Gold = 999
        };

        // Act
        var actual = command.Execute(Substitute.For<IConsole>(), null, saveData);

        // Assert
        Assert.That(actual, Is.True);
        Assert.That(saveData.Gold, Is.EqualTo(999 - cost));
        Assert.That(saveData.Party.TrueForAll(c => c.CurrentHealth == c.TotalHealth));
    }
}

using NUnit.Framework;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Tests.Characters;


[TestFixture]
public class CharacterTests
{
    [Test]
    public void Defend_SetsIsDefendingToTrue()
    {
        // Arrange
        var c = new Character("Muhammad", 100, 10, 3);

        // Act
        c.Defend();

        // Assert
        Assert.That(c.IsDefending, Is.True);
    }

    [Test]
    public void OnRoundComplete_SetsIsDefendingToFalse()
    {
        // Arrange
        var c = new Character("Muhammad", 100, 10, 3);
        c.Defend();

        // Act
        c.OnRoundComplete();

        // Assert
        Assert.That(c.IsDefending, Is.False);
    }
}

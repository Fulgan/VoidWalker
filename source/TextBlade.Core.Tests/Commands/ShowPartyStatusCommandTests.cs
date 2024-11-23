using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ShowPartyStatusCommandTests
{
    [Test]
    public void Execute_ReportsHealthPerCharacter()
    {
        // Arrange
        var party = new List<Character>
        {
            new("Bilal", 10, 25),
            new("Aisha", 103, 110)
        };

        var command = new ShowPartyStatusCommand();

        // Act
        var actual = command.Execute(null, party);

        // Assert
        Assert.That(actual.Any(a => a.StartsWith("Party status")));
        Assert.That(actual.Any(a => a.Contains("Bilal: 10/25")));
        Assert.That(actual.Any(a => a.Contains("Aisha: 103/110")));
    }
}

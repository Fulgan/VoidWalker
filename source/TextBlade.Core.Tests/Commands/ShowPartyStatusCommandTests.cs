using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.Tests.TestHelpers;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ShowPartyStatusCommandTests
{
    [Test]
    public async Task Execute_ReportsHealthPerCharacter()
    {
        // Arrange
        var party = new List<Character>
        {
            new Character("Bilal", 10, 25, 0),
            new Character("Aisha", 103, 110, 0)
        };

        var command = new ShowPartyStatusCommand();

        // Act
        var messages = command.Execute(null, party);
        var actual = await AsyncToList.ToList(messages);

        // Assert
        Assert.That(actual.Any(a => a.StartsWith("Party status")));
        Assert.That(actual.Any(a => a.Contains("Bilal: 10/25")));
        Assert.That(actual.Any(a => a.Contains("Aisha: 103/110")));
    }
}

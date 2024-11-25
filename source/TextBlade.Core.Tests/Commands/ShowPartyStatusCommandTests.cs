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
            CharacterMaker.CreateCharacter("Bilal", 10, 25),
            CharacterMaker.CreateCharacter("Aisha", 103, 110)
        };

        var command = new ShowPartyStatusCommand();

        // Act
        var actual = await AsyncToList.ToList(command.Execute(null, party));

        // Assert
        Assert.That(actual.Any(a => a.StartsWith("Party status")));
        Assert.That(actual.Any(a => a.Contains("Bilal: 10/25")));
        Assert.That(actual.Any(a => a.Contains("Aisha: 103/110")));
    }
}

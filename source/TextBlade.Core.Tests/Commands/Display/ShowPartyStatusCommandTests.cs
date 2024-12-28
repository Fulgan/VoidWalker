using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.IO;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Commands.Display;

[TestFixture]
public class ShowPartyStatusCommandTests
{
    [Test]
    public void Execute_ReportsHealthPerCharacter()
    {
        // Arrange
        var party = new List<Character>
        {
            new Character("Bilal", 25, 0, 0,  0, 0, 0) { CurrentHealth = 10 },
            new Character("Aisha", 110, 0, 0,  0, 0, 0) { CurrentHealth = 103 },
        };

        var console = new ConsoleStub();
        var command = new ShowPartyStatusCommand(console);

        // Act
        command.Execute(new SaveData() { Party = party});

        // Assert
        Assert.That(console.Messages.Any(a => a.StartsWith("Party status")));
        Assert.That(console.Messages.Any(a => a.Contains("Bilal: 10/25 health")));
        Assert.That(console.Messages.Any(a => a.Contains("Aisha: 103/110 health")));
    }
}

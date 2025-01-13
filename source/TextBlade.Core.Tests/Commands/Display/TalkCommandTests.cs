using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.Locations;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Commands.Display;

[TestFixture]
public class TalkCommandTests
{
    [Test]
    public void Execute_ReturnsNobodyMessage_IfThereAreNoNpcs()
    {
        // Arrange
        var command = new TalkCommand();
        var console = new ConsoleStub();
        var location = new Location("Abandoned Factory", "It's abandoned!");

        // Act
        command.Execute(console, location, null);

        // Assert
        Assert.That(console.Messages.Any(m => m.Contains("nobody")));
    }

    [Test]
    public void Execute_TalksToTheOneAndOnlyNpc()
    {
        // Arrange
        var command = new TalkCommand();
        var console = new ConsoleStub();
        var location = new Location("Derelict Factory", "It's run-down ...")
        {
            Npcs = [new Npc("Robot", ["HELLO, I AM ROBOT!"])]
        };

        // Act
        command.Execute(console, location, null);

        // Assert
        Assert.That(console.Messages.Any(m => m.Contains("I AM ROBOT")));
    }

    [Test]
    public void Execute_TalksToSpecifiedNpc_IfThereAreMultiple()
    {
        // Arrange
        var command = new TalkCommand();
        var console = new ConsoleStub();
        var location = new Location("Derelict Factory", "It's run-down ...")
        {
            Npcs =
            [
                new Npc("Robot First", ["FAIL!"]),
                new Npc("Robot Second", ["PASS!"]),
                new Npc("Robot Third", ["FAIL!"]),
            ]
        };

        console.PressKey('2');

        // Act
        command.Execute(console, location, null);

        // Assert
        Assert.That(console.Messages.Any(m => m.Contains("Robot Second")));
        Assert.That(console.Messages.Any(m => m.Contains("PASS")));
    }
}

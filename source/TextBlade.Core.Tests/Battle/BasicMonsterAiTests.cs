using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Battle;

[TestFixture]
public class BasicMonsterAiTests
{
    [Test]
    public void ProcessTurnFor_DoesNothing_IfPartyIsDead()
    {
        // Arrange
        var party = new List<Character>
        {
            new Character("Target A", 0, 100, toughness: 10),
            new Character("Target B", 0, 100, toughness: 7),
        };

        var ai = new BasicMonsterAi(Substitute.For<IConsole>(), party);

        // Act.
        for (int i = 0; i < 10; i++)
        {
            ai.ProcessTurnFor(new Monster("Attacker-Sama", 100, 15, 0));
        }

        // Assert
        Assert.That(party[0].CurrentHealth, Is.EqualTo(0));
        Assert.That(party[1].CurrentHealth, Is.EqualTo(0));
    }

    [Test]
    public void ProcessTurnFor_AttacksARandomPartyMember()
    {
        // Arrange
        var party = new List<Character>
        {
            new Character("Target A", 100, 100, toughness: 10),
            new Character("Target B", 100, 100, toughness: 7),
        };

        var ai = new BasicMonsterAi(Substitute.For<IConsole>(), party);
        var attacker = new Monster("Attacker-Sama", 100, 15, 0);

        // Act. Do it a few times. Because random is random.
        for (int i = 0; i < 10; i++)
        {
            ai.ProcessTurnFor(attacker);
        }

        // Assert
        Assert.That(party[0].CurrentHealth != party[0].TotalHealth);
        Assert.That(party[1].CurrentHealth != party[1].TotalHealth);
    }

    [Test]
    public void ProcessTurnFor_NotifiesIfAPlayerDies()
    {
        // Arrange
        var targetName = "Glass Boi";
        var party = new List<Character>
        {
            new Character(targetName, 1, 1, toughness: 7),
        };

        var console = new ConsoleStub();
        var ai = new BasicMonsterAi(console, party);
        var attacker = new Monster("Stone Monster", 100, 999, 999);

        // Act
        ai.ProcessTurnFor(attacker);

        // Assert
        Assert.That(console.LastMessage, Does.Contain($"DIES"));
    }
}

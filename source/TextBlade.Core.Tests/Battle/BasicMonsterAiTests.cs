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
            new Character("Target A", 0, 100, 10, 0, 0, 0),
            new Character("Target B", 0, 100, 7, 0, 0, 0, 0),
        };

        var ai = new BasicMonsterAi(Substitute.For<IConsole>(), party);

        // Act.
        for (int i = 0; i < 10; i++)
        {
            ai.ProcessTurnFor(new Monster("Attacker-Sama", 100, 15, 0, 0, 0, 0, 0));
        }

        // Assert
        Assert.That(party[0].CurrentHealth, Is.EqualTo(0));
        Assert.That(party[1].CurrentHealth, Is.EqualTo(0));
    }

    [Test]
    public void ProcessTurnFor_AttacksARandomAlivePartyMember()
    {
        // Arrange
        var party = new List<Character>
        {
            new Character("Target A", 100, 100, 10, 0, 0, 0, 0),
            new Character("Target B", 100, 100, 7, 0, 0, 0, 0),
            new Character("Dead Duck", 100, 100, 7, 0, 0, 0, 0) { CurrentHealth = 0 },
            new Character("Dead Duck 2", 100, 100, 7, 0, 0, 0, 0) { CurrentHealth = 0 },
            new Character("Dead Duck 3", 100, 100, 7, 0, 0, 0, 0) { CurrentHealth = 0 },
        };

        var console = new ConsoleStub();
        var ai = new BasicMonsterAi(console, party);
        var attacker = new Monster("Attacker-Sama", 100, 15, 0, 0, 0, 0, 0);

        // Act. Do it a few times. Because random is random.
        for (int i = 0; i < 10; i++)
        {
            ai.ProcessTurnFor(attacker);
        }

        // Assert
        Assert.That(party[0].CurrentHealth != party[0].TotalHealth);
        Assert.That(party[1].CurrentHealth != party[1].TotalHealth);
        // Didn't target our duckies
        Assert.That(console.Messages.All(m => !m.Contains("Dead Duck")));
    }

    [Test]
    public void ProcessTurnFor_LogsIfAPlayerDies()
    {
        // Arrange
        var targetName = "Glass Boi";
        var party = new List<Character>
        {
            new Character(targetName, 1, 1, 7, 0, 0, 0, 0),
        };

        var console = new ConsoleStub();
        var ai = new BasicMonsterAi(console, party);
        var attacker = new Monster("Stone Monster", 100, 999, 999, 0, 0, 0, 0);

        // Act
        ai.ProcessTurnFor(attacker);

        // Assert
        Assert.That(console.LastMessage, Does.Contain($"DIES"));
    }

    [Test]
    public void ProcessTurnFor_UsesSkillsSometimes_IfMonsterHasEnoughSkillPoints()
    {
        // Arrange
        var target = new Character("Infinite Test Dummy", 999999999, 0, 999999, 0, 0, 0);
        var attacker = new Monster("Spider", 100, 100, 100, 100, 100, 100, 100);
        var skillName = "Web"; // Make sure it's in Skills.json
        
        // Not sure how this is usually set up. Herp derp...
        attacker.SkillNames.Add(skillName);
        attacker.Skills.Add(new Skill()
        {
            Cost = 10,
            DamageMultiplier = 2.0f,
            DamageType = "Bizarre",
            Name = skillName,
        });
        attacker.SkillProbabilities[skillName] = 0.8;

        var console = new ConsoleStub();

        // Act
        for (int i = 0; i < 10; i++)
        {
            new BasicMonsterAi(console, [target]).ProcessTurnFor(attacker);
        }

        // Assert
        Assert.That(console.Messages.Any(m => m.Contains($"uses [#faa]{skillName} on {target.Name}[/]")));
        Assert.That(console.Messages.Any(m => m.Contains($"attacks {target.Name}"))); // Didn't skill ALL the time. Not enough skill points for that.
    }
}

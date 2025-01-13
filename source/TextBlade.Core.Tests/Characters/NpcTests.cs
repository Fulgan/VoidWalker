using NUnit.Framework;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Tests.Characters;

[TestFixture]
public class NpcTests
{
    [Test]
    public void Speak_ReturnsTheOnlyMessage()
    {
        // Arrange
        var expected = "Spawn more overlords!";
        var npc = new Npc("Overlord 1", [expected]);

        // Act/Assert
        for (int i = 0; i < 10; i++)
        {
            var actual = npc.Speak();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [Test]
    public void Speak_CyclesThroughMessages()
    {
        var expected = new string[] {
            "Message 1",
            "Message 2",
            "Message 3"
        };

        var npc = new Npc("Speaker for the Speakers", expected);

        // Act/Assert
        Assert.That(npc.Speak(), Is.EqualTo(expected[0]));
        Assert.That(npc.Speak(), Is.EqualTo(expected[1]));
        Assert.That(npc.Speak(), Is.EqualTo(expected[2]));
        
        Assert.That(npc.Speak(), Is.EqualTo(expected[0]));
        Assert.That(npc.Speak(), Is.EqualTo(expected[1]));
        Assert.That(npc.Speak(), Is.EqualTo(expected[2]));
    }
}

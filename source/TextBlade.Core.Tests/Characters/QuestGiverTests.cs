using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Tests.Characters;

[TestFixture]
public class QuestGiverTests
{
    [Test]
    public void Speak_CyclesThroughMessages_IfSwitchIsntSet()
    {
        // Arrange
        var regularMessages = new string[] { "Stay a while, and listen ...", "Beware the Butcher!" };
        var giver = new QuestGiver("Kain", regularMessages, [], "Switch_That_DoesntExist");

        // Act/Assert
        Assert.That(giver.Speak(), Is.EqualTo(regularMessages[0]));
        Assert.That(giver.Speak(), Is.EqualTo(regularMessages[1]));
        Assert.That(giver.Speak(), Is.EqualTo(regularMessages[0]));
        Assert.That(giver.Speak(), Is.EqualTo(regularMessages[1]));
    }

    [Test]
    public void Speak_SetsSwitch_IfItIsntSet()
    {
        // Arrange
        var expectedSwitchName = GameSwitches.GetTalkedToSwitchForQuestGiver("Kabil");
        var giver = new QuestGiver("Kabil", ["I will kill you"], [], expectedSwitchName);

        // Act
        giver.Speak();

        // Assert
        Assert.That(GameSwitches.Switches.Has(expectedSwitchName));       
        Assert.That(GameSwitches.Switches.Get(expectedSwitchName), Is.True);       
    }

    [Test]
    public void Speak_GetsPostQuestMessages_IfSwitchIsSet()
    {
        // Arrange
        var postQuestMessages = new string[] { "Stay a while, and listen ...", "Beware the Butcher!" };
        var giver = new QuestGiver("K3", ["You shouldn't see this"], postQuestMessages, "TalkedTo_K3");
        var expectedSwitchName = GameSwitches.GetTalkedToSwitchForQuestGiver("K3");
        GameSwitches.Switches.Set(expectedSwitchName, true);       

        // Act/Assert
        Assert.That(giver.Speak(), Is.EqualTo(postQuestMessages[0]));
        Assert.That(giver.Speak(), Is.EqualTo(postQuestMessages[1]));
        Assert.That(giver.Speak(), Is.EqualTo(postQuestMessages[0]));
        Assert.That(giver.Speak(), Is.EqualTo(postQuestMessages[1]));
    }
}

using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.Game;
using TextBlade.Core.Locations;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Locations;

[TestFixture]
public class LocationTests
{
    [Test]
    public void GetCommandFor_ReturnDoNothingCommand()
    {
        // Arrange
        var location = new Location("A", "B");

        // Act
        var actual = location.GetCommandFor("q");

        // Assert
        Assert.That(actual, Is.InstanceOf<DoNothingCommand>());
    }

    [Test]
    public void GetExtraDescription_ReturnsEmptyString()
    {
        // Arrange
        var location = new Location("A", "B");

        // Act
        var actual = location.GetExtraDescription();

        // Assert
        Assert.That(actual, Is.Empty);
    }

    [Test]
    public void GetExtraMenuOption_ReturnsEmptyString()
    {
        // Arrange
        var location = new Location("A", "B");

        // Act
        var actual = location.GetExtraMenuOptions();

        // Assert
        Assert.That(actual, Is.Empty);
    }

    [Test]
    public void VisibleLocations_GetsAllNormalLinkedLocations()
    {
        // Arrange
        var location = new Location("Desert", "Desserted, get it?!")
        {
            LinkedLocations = new List<LocationLink>
            {
                new ("north", "North"),
                new ("South", "South"),
            }
        };

        // Act
        var actual = location.VisibleLocations;

        // Assert
        Assert.That(actual, Is.EqualTo(location.LinkedLocations));
    }

    [Test]
    public void VisibleLocations_DoesNotListLinkedLocations_IfSwitchIsMissingOrFalse()
    {
        // Arrange
        var location = new Location("Castle in the Clouds", "Very bright and sunny eh?")
        {
            LinkedLocations = new List<LocationLink>
            {
                new ("Front Entrance", "A huge gate bars the entrance"),
                new ("Secret Hideaway", "Its a sekrit", "FoundSecretHideaway"),
                new ("Merchant Nest", "A group of merchants stand around", "RescuedMerchants")
            }
        };

        // FoundSecretHideaway isn't set at all
        GameSwitches.Switches.Set("RescuedMerchants", false);

        // Act
        var actual = location.VisibleLocations;

        // Assert
        Assert.That(actual.Count(), Is.EqualTo(1));
        Assert.That(actual.Single(), Is.EqualTo(location.LinkedLocations.First()));
    }
}

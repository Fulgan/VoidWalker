using System;
using NUnit.Framework;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.IO;

[TestFixture]
public class SerializerTests
{
    [Test]
    public void Deserialize_CorrectlyDerializesSmallVIllage()
    {
        // Arrangement is done in the JSON files
        var filePath = Path.Join("TestData", "Regions", "VillageWithInnAndShops.json");
        var json = File.ReadAllText(filePath);
        var actual = Serializer.Deserialize<Region>(json);

        // Assert
        Assert.That(actual, Is.Not.Null);

        Assert.That(actual.Name, Is.EqualTo("King's Vale"));
        Assert.That(actual.Description, Does.Contain("small village"));

        Assert.That(actual.ReachableRegions.Count, Is.EqualTo(3));
        Assert.That(actual.ReachableRegions[0].Id, Is.EqualTo("KingsVale/Inn"));
        Assert.That(actual.ReachableRegions[0].Description, Is.EqualTo("The King's Inn"));
        Assert.That(actual.ReachableRegions[1].Id, Is.EqualTo("KingsVale/EquipmentShop"));
        Assert.That(actual.ReachableRegions[1].Description, Is.EqualTo("Bronzebeard's Wares"));
        Assert.That(actual.ReachableRegions[2].Id, Is.EqualTo("KingsVale/ItemShop"));
        Assert.That(actual.ReachableRegions[2].Description, Is.EqualTo("Potions R Us"));

    }
}

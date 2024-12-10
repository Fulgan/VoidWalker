using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.IO;

[TestFixture]
public class SerializerTests
{
    [Test]
    public void Serialize_IncludesTypeInfo()
    {
        // Arrange
        var kingdom = new Location("Bob's Kingdom", "KING BOB!!!!");

        // Act
        var actual = Serializer.Serialize(kingdom);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Does.Contain("\"$type\":"));
        Assert.That(actual, Does.Contain("TextBlade.Core.Locations.Location, TextBlade.Core"));
    }

    [Test]
    public void Deserialize_CorrectlyDerializesSmallVIllage()
    {
        // Arrangement is done in the JSON files
        var filePath = Path.Join("TestData", "Locations", "VillageWithInnAndShops.json");
        var json = File.ReadAllText(filePath);
        var actual = Serializer.Deserialize<Location>(json);

        // Assert
        Assert.That(actual, Is.Not.Null);

        Assert.That(actual.Name, Is.EqualTo("King's Vale"));
        Assert.That(actual.Description, Does.Contain("small village"));

        Assert.That(actual.LinkedLocations.Count, Is.EqualTo(3));
        Assert.That(actual.LinkedLocations[0].Id, Is.EqualTo("KingsVale/Inn"));
        Assert.That(actual.LinkedLocations[0].Description, Is.EqualTo("The King's Inn"));
        Assert.That(actual.LinkedLocations[1].Id, Is.EqualTo("KingsVale/EquipmentShop"));
        Assert.That(actual.LinkedLocations[1].Description, Is.EqualTo("Bronzebeard's Wares"));
        Assert.That(actual.LinkedLocations[2].Id, Is.EqualTo("KingsVale/ItemShop"));
        Assert.That(actual.LinkedLocations[2].Description, Is.EqualTo("Potions R Us"));
    }

    [Test]
    public void DeserializeParty_CorrectlyDeserializesPartyMembers()
    {
        // Arrange is done in the JSON files
        var filePath = Path.Join("TestData", "Characters", "TwoMemberParty.json");
        var json = File.ReadAllText(filePath);
        var jArray = Serializer.Deserialize<JArray>(json);
        var actual = Serializer.DeserializeParty(jArray);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Count, Is.EqualTo(2));
        var ahmed = actual[0];
        Assert.That(ahmed.Name, Is.EqualTo("Ahmed"));
        Assert.That(ahmed.TotalHealth, Is.EqualTo(100));
        Assert.That(ahmed.CurrentHealth, Is.EqualTo(10));
        var bilal = actual[1];
        Assert.That(bilal.Name, Is.EqualTo("Bilal"));
        Assert.That(bilal.TotalHealth, Is.EqualTo(75));
        Assert.That(bilal.CurrentHealth, Is.EqualTo(66));
    }

    [Test]
    public void Deserialize_CorrectlyDeserializesTownAsLocationn_WhenTypeIsSpecified()
    {
        // Arrange is done in the JSON files
        var filePath = Path.Join("TestData", "Locations", "InnWithType.json");
        var json = File.ReadAllText(filePath);
        var actual = Serializer.Deserialize<Location>(json);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.TypeOf<Inn>());
        var inn = actual as Inn;
        Assert.That(inn.InnCost, Is.EqualTo(10));
    }

    [Test]
    public void Deserialize_CorrectlyDeserializesDungeonFloorLoot()
    {
        // Arrange is done in the JSON files
        var filePath = Path.Join("TestData", "Locations", "DungeonWithFloorLoot.json");
        var json = File.ReadAllText(filePath);
        var actual = Serializer.Deserialize<Location>(json);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.TypeOf<Dungeon>());
        var dungeon = actual as Dungeon;

        Assert.That(dungeon.FloorLoot, Is.Not.Null);
        Assert.That(dungeon.FloorLoot.Count, Is.EqualTo(2));

        var b2Loot = dungeon.FloorLoot["B2"];
        Assert.That(b2Loot.Contains("Iron Sword"));
        Assert.That(b2Loot.Contains("Iron Shield"));
        Assert.That(b2Loot.Count(c => c == "Potion"), Is.EqualTo(2));

        
        var b5Loot = dungeon.FloorLoot["B5"];
        Assert.That(b5Loot.Contains("Iron Shield"));
        Assert.That(b5Loot.Count(c => c == "High-Potion"), Is.EqualTo(3));
    }

    [Test]
    public void Deserialize_CorrectlyDeserializesEquipment()
    {
        // Arrange
        var expectedHelmet = new Equipment("Plumed Hat", ItemType.Helmet.ToString(), new Dictionary<CharacterStats, int> { { CharacterStats.Toughness, 5  } });
        var expectedArmour = new Equipment("Kimono", ItemType.Armour.ToString(), new Dictionary<CharacterStats, int> { { CharacterStats.Toughness, 3  } });
        var expectedWeapon = new Equipment("Dirk", ItemType.Weapon.ToString(), new Dictionary<CharacterStats, int> { { CharacterStats.Strength, 7  } });

        // Act
        var serialized = new string[]
        {
            Serializer.Serialize(expectedHelmet),
            Serializer.Serialize(expectedArmour),
            Serializer.Serialize(expectedWeapon)
        };

        var actual = new Equipment[]
        {
            Serializer.Deserialize<Equipment>(serialized[0]),
            Serializer.Deserialize<Equipment>(serialized[1]),
            Serializer.Deserialize<Equipment>(serialized[2]),
        };

        // Assert
        var actualHelmet = actual[0];
        Assert.That(actualHelmet.Name, Is.EqualTo(expectedHelmet.Name));
        Assert.That(actualHelmet.ItemType, Is.EqualTo(ItemType.Helmet));
        Assert.That(actualHelmet.ToString(), Does.Contain("Toughness +5"));

        var actualArmour = actual[1];
        Assert.That(actualArmour.Name, Is.EqualTo(expectedArmour.Name));
        Assert.That(actualArmour.ItemType, Is.EqualTo(ItemType.Armour));
        Assert.That(actualArmour.ToString(), Does.Contain("Toughness +3"));

        var actualWeapon = actual[2];
        Assert.That(actualWeapon.Name, Is.EqualTo(expectedWeapon.Name));
        Assert.That(actualWeapon.ItemType, Is.EqualTo(ItemType.Weapon));
        Assert.That(actualWeapon.ToString(), Does.Contain("Strength +7"));
    }

    [Test]
    public void Deserialize_PreservesEquipmentType_ForInventoryItems()
    {
        // Arrange
        var inventory = new Inventory();
        inventory.Add(new Equipment(
            "Bandanna",
            ItemType.Helmet.ToString(),
            new Dictionary<CharacterStats, int>
            {
                { CharacterStats.Toughness, 11 },
            }));

        // Act
        var serialized = Serializer.Serialize(inventory);
        var actual = Serializer.Deserialize<Inventory>(serialized);

        // Assert
        var actualHelmet = actual.NameToData["Bandanna"];
        Assert.That(actualHelmet, Is.TypeOf<Equipment>());
        Assert.That(actualHelmet.ToString(), Does.Contain("Toughness +11"));
    }

    [Test]
    public void Deserialize_DeserializesEquipmentAsCorrectType_WhenTypeIsInJson()
    {
        // Arrange is done in the JSON files
        var filePath = Path.Join("TestData", "Saves", "SaveDataWithEquipment.json");
        var json = File.ReadAllText(filePath);
        var actual = Serializer.Deserialize<SaveData>(json);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Inventory, Is.Not.Null);
        var actualInventory = actual.Inventory;
        Assert.That(actualInventory.NameToData["Iron Sword"], Is.InstanceOf<Equipment>());
        Assert.That(actualInventory.NameToData["Iron Helmet"], Is.InstanceOf<Equipment>());
        Assert.That(actualInventory.NameToData["Iron Armour"], Is.InstanceOf<Equipment>());
    }
}

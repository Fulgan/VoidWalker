using System;
using NUnit.Framework;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Locations;

[TestFixture]
public class DungeonTests
{
    // Bro. You have a *lot* of test-cases to catch up on.

    [Test]
    [TestCase(-1)]
    [TestCase(-11)]
    [TestCase(-111111)]
    public void SetState_ThrowsIfFloorNumberIsNegative(int floorNumber)
    {
        // Arrange
        var dungeon = new Dungeon("Test Dungeon", "N/A", 7, ["Troll"], "Nobody");
        
        // Act/Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => dungeon.SetState(floorNumber, true));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public void SetState_SetsCurrentFloorNumber()
    {
        // Arrange
        var dungeon = new Dungeon("North Seaside Cave", "N/A", 1, ["Vole"], "Nobody");

        // Act
        dungeon.SetState(33, false);

        // Assert
        Assert.That(dungeon.CurrentFloorNumber, Is.EqualTo(33));
    }

    [Test]
    public void SetState_DoesNotClearMonstersOrLoot_IfIsClearIsFalse()
    {
        // Arrange
        var floorNumber = 0;
        var dungeon = new Dungeon("South Seaside Cave", "N/A", 1, ["Mole"], "Nobody");
        dungeon.FloorLoot[$"B{floorNumber + 1}"] = ["Troll Hair", "Vole Tail", "Moleskin"];

        // Act
        dungeon.SetState(floorNumber, false);

        // Assert
        Assert.That(dungeon.FloorLoot[$"B{floorNumber + 1}"], Is.Not.Empty);
        Assert.That(dungeon.GetExtraDescription(), Does.Contain("Mole"));
    }

    [Test]
    public void SetState_ClearsMonstersOrLoot_IfIsClearIsTrue()
    {
        // Arrange
        var floorNumber = 0;
        var dungeon = new Dungeon("South Seaside Cave", "N/A", 3, ["Mole"], "Nobody");
        dungeon.FloorLoot[$"B{floorNumber + 1}"] = ["Troll Hair"];
        dungeon.FloorLoot[$"B{floorNumber + 2}"] = ["Vole Tail", "Moleskin"];

        // Act
        dungeon.SetState(floorNumber, true);

        // Assert
        Assert.That(dungeon.FloorLoot[$"B{floorNumber + 1}"], Is.Empty);
        Assert.That(dungeon.GetExtraDescription(), Does.Not.Contain("Mole"));
        // Didn't clear other floors
        Assert.That(dungeon.FloorLoot[$"B{floorNumber + 2}"], Is.Not.Empty);
    }
}

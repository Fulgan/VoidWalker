using NUnit.Framework;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Locations;

[TestFixture]
public class DungeonTests
{
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-123)]
    public void Constructor_Throws_IfNumFloorsIsNotPositive(int numFloors)
    {
        // Arrange/Act/Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Dungeon("Lava Dungeon", "Mmm, ice-cream", numFloors, ["A", "B"], "Bossman 37"));
        Assert.That(ex.Message, Does.Contain(nameof(numFloors)));
    }

    [Test]
    public void Constructor_Throws_IfMonsterListIsEmpty()
    {
        // Arrange/Act/Assert
        var ex = Assert.Throws<ArgumentException>(() => new Dungeon("Ice Dungeon", "Mmm, stew", 33, [], "Bossman 38"));
        Assert.That(ex.Message, Does.Contain("monsters"));
    }

    [Test]
    public void Constructor_AddsMonsters_ToEveryFloor()
    {
        // Arrange/Act
        int numFloors = 5;
        var dungeon = new DungeonStub("Real Dungeon #1", "It's real I swear", numFloors, ["Blue Slime", "Green Slime", "Red Slime"], "Giant Slime");

        // Assert
        for (int i = 0; i < numFloors; i++)
        {
            var actualMonsters = dungeon.GetMonstersOnFloor(i);
            Assert.That(actualMonsters.Any());
            Assert.That(actualMonsters.All(a => a.Contains("Slime")));
        }
    }

    [Test]
    public void Constructor_AddsWeakerMonstersToLowerFloorAndStrongerToHigherFloorsAndAFinalBossAtTheEnd()
    {
        // Arrange/Act
        int numFloors = 5;
        var dungeon = new DungeonStub("Real Dungeon #1", "It's real I swear", numFloors, ["Blue Slime", "Green Slime", "Red Slime"], "Giant Slime");

        // Assert
        Assert.That(dungeon.GetMonstersOnFloor(0).All(a => a == "Blue Slime" || a == "Green Slime"));
        var finalFloorMonsters = dungeon.GetMonstersOnFloor(numFloors - 1);
        // Everyone's a final slime, except for the big boss
        Assert.That(finalFloorMonsters.Count(a => a == "Red Slime"), Is.EqualTo(finalFloorMonsters.Count - 1));
        Assert.That(finalFloorMonsters, Does.Contain("Giant Slime"));
    }

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

    class DungeonStub : Dungeon
    {
        public DungeonStub(string name, string description, int numFloors, List<string> monsters, string boss, string? locationClass = null) : base(name, description, numFloors, monsters, boss, locationClass)
        {
        }

        public List<string> GetMonstersOnFloor(int floor)
        {
            return base._floorMonsters[floor];
        }
    }
}

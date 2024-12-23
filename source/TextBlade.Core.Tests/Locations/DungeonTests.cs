using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;
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
        var dungeon = CreateDungeon(numFloors);

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
        var dungeon = CreateDungeon(numFloors);

        // Assert
        Assert.That(dungeon.GetMonstersOnFloor(0).All(a => a == "Blue Slime" || a == "Green Slime"));
        var finalFloorMonsters = dungeon.GetMonstersOnFloor(numFloors - 1);
        // Everyone's a final slime, except for the big boss
        Assert.That(finalFloorMonsters.Count(a => a == "Red Slime"), Is.EqualTo(finalFloorMonsters.Count - 1));
        Assert.That(finalFloorMonsters, Does.Contain("Giant Slime"));
    }

    [Test]
    public void OnVictory_ClearsCurrentFloorMonsters()
    {
        // Arrange
        var dungeon = CreateDungeon();

        // Act
        dungeon.OnVictory();

        // Assert
        Assert.That(dungeon.GetMonstersOnFloor(0).Count, Is.EqualTo(0));
    }

    [Test]
    [TestCase(-1)]
    [TestCase(-11)]
    [TestCase(-111111)]
    public void SetStateBasedOnCustomSaveData_ThrowsIfFloorNumberIsNegative(int floorNumber)
    {
        // Arrange
        var dungeon = new Dungeon("Test Dungeon", "N/A", 7, ["Troll"], "Nobody");
        
        // Act/Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(floorNumber, true)));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public void SetStateBasedOnCustomSaveData_SetsCurrentFloorNumber()
    {
        // Arrange
        var dungeon = new Dungeon("North Seaside Cave", "N/A", 40, ["Vole"], "Nobody");

        // Act
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(33, false));

        // Assert
        Assert.That(dungeon.GetCustomSaveData()["CurrentFloor"], Is.EqualTo(33));
    }

    [Test]
    public void SetStateBasedOnCustomSaveData_DoesNotClearMonstersOrLoot_IfIsClearIsFalse()
    {
        // Arrange
        var floorNumber = 0;
        var dungeon = new Dungeon("South Seaside Cave", "N/A", 1, ["Mole"], "Nobody");
        dungeon.FloorLoot[$"B{floorNumber + 1}"] = ["Troll Hair", "Vole Tail", "Moleskin"];

        // Act
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(floorNumber, false));

        // Assert
        Assert.That(dungeon.FloorLoot[$"B{floorNumber + 1}"], Is.Not.Empty);
        Assert.That(dungeon.GetExtraDescription(), Does.Contain("Mole"));
    }

    [Test]
    public void SetStateBasedOnCustomSaveData_ClearsMonstersOrLoot_IfIsClearIsTrue()
    {
        // Arrange
        var floorNumber = 0;
        var dungeon = new Dungeon("South Seaside Cave", "N/A", 3, ["Mole"], "Nobody");
        dungeon.FloorLoot[$"B{floorNumber + 1}"] = ["Troll Hair"];
        dungeon.FloorLoot[$"B{floorNumber + 2}"] = ["Vole Tail", "Moleskin"];

        // Act
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(floorNumber, true));

        // Assert
        Assert.That(dungeon.FloorLoot[$"B{floorNumber + 1}"], Is.Empty);
        Assert.That(dungeon.GetExtraDescription(), Does.Not.Contain("Mole"));
        // Didn't clear other floors
        Assert.That(dungeon.FloorLoot[$"B{floorNumber + 2}"], Is.Not.Empty);
    }

    [Test]
    public void GetExtraDescription_ListsMonsters_IfThereAreAny()
    {
        // Arrange
        var dungeon = CreateDungeon();
        
        // Act
        var actual = dungeon.GetExtraDescription();

        // Assert
        Assert.That(actual.Contains("Green Slime") || actual.Contains("Blue Slime"));
    }

    [Test]
    public void GetExtraDescription_TellsYouThereAreNoMonsters_IfCleared()
    {
        // Arrange
        var dungeon = CreateDungeon();
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(0, true));
        
        // Act
        var actual = dungeon.GetExtraDescription();

        // Assert
        Assert.That(actual, Does.Contain("no monsters"));
    }

    [Test]
    public void GetExtraDescription_HintsAtTreasure_IfThereIsAny()
    {
        // Arrange
        var dungeon = CreateDungeon();
        dungeon.FloorLoot["B1"] = new List<string>
        {
            "100 gold",
            "A gilded bone"
        };
        
        // Act
        var actual = dungeon.GetExtraDescription();

        // Assert
        Assert.That(actual, Does.Contain("something shiny"));
    }

    [Test]
    public void GetExtraDescription_TellsYouTheCurrentFloorNumber()
    {
        // Arrange
        var dungeon = CreateDungeon();
        
        // Act
        var actual = dungeon.GetExtraDescription();

        // Assert
        Assert.That(actual, Does.Contain("You are on floor 1"));
    }

    [Test]
    public void GetExtraMenuOptions_TellsYouYouCanDescend_IfTheFloorIsEmpty()
    {
        // Arrange
        var dungeon = CreateDungeon();
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(0, true));

        // Act
        var actual = dungeon.GetExtraMenuOptions();

        // Assert
        Assert.That(actual, Does.Contain("go to the next floor"));
    }

    [Test]
    public void GetExtraMenuOptions_DoesNotTellYouYouCanDescend_IfTheFloorIsEmptyButItsTheLastFloor()
    {
        // Arrange
        var dungeon = CreateDungeon(1);
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(0, true));

        // Act
        var actual = dungeon.GetExtraMenuOptions();

        // Assert
        Assert.That(actual, Does.Not.Contain("go to the next floor"));
    }

    [Test]
    public void GetExtraMenuOptions_TellsYouToFight_IfFloorIsNotEmpty()
    {
        // Arrange
        var dungeon = CreateDungeon(1);

        // Act
        var actual = dungeon.GetExtraMenuOptions();

        // Assert
        Assert.That(actual, Does.Contain("fight"));
    }

    [Test]
    [TestCase("f")]
    [TestCase("fight")]
    public void GetCommandFor_ReturnsTakeTurnsBattleCommand(string command)
    {
        // Arrange
        var dungeon = CreateDungeon();

        // Act
        var actual = dungeon.GetCommandFor(Substitute.For<IConsole>(), command);

        // Assert
        Assert.That(actual, Is.InstanceOf<FightCommand>());
    }

    [Test]
    [TestCase("d")]
    [TestCase("down")]
    [TestCase("descend")]
    public void GetCommandFor_IncrementsFloorNumber_IfYouCanDescend(string command)
    {
        // Arrange
        var dungeon = CreateDungeon();
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(0, true));

        // Act
        dungeon.GetCommandFor(Substitute.For<IConsole>(), command);

        // Assert
        Assert.That(dungeon.GetCustomSaveData()["CurrentFloor"], Is.EqualTo(1)); // base 0
    }

    [Test]
    [TestCase("d")]
    [TestCase("down")]
    [TestCase("descend")]
    public void GetCommandFor_ReturnsDoNothingCommandForDescend_IfThereAreMonsters(string command)
    {
        // Arrange
        var dungeon = CreateDungeon();

        // Act
        var actual = dungeon.GetCommandFor(Substitute.For<IConsole>(), command);

        // Assert
        Assert.That(dungeon.GetCustomSaveData()["CurrentFloor"], Is.EqualTo(0));
        Assert.That(actual, Is.InstanceOf<DoNothingCommand>());
    }

    [Test]
    [TestCase("d")]
    [TestCase("down")]
    [TestCase("descend")]
    public void GetCommandFor_ReturnsDoNothingCommandForDescend_IfItsTheBottomFloor(string command)
    {
        // Arrange
        var dungeon = CreateDungeon(1);
        dungeon.SetStateBasedOnCustomSaveData(MakeCustomData(0, true));

        // Act
        var actual = dungeon.GetCommandFor(Substitute.For<IConsole>(), command);

        // Assert
        Assert.That(dungeon.GetCustomSaveData()["CurrentFloor"], Is.EqualTo(0));
        Assert.That(actual, Is.InstanceOf<DoNothingCommand>());
    }


    private DungeonStub CreateDungeon(int numFloors = 5)
    {
        var dungeon = new DungeonStub("Real Dungeon #1", "It's real I swear", numFloors, ["Blue Slime", "Green Slime", "Red Slime"], "Giant Slime");
        return dungeon;
    }

    private Dictionary<string, object> MakeCustomData(int floorNum, bool isFloorClear)
    {
        return new Dictionary<string, object>() {
            { "CurrentFloor", floorNum },
            { "IsClear", isFloorClear },
        };
    }

    class DungeonStub : Dungeon
    {
        public DungeonStub(string name, string description, int numFloors, List<string> monsters, string boss, string? locationClass = null) 
            : base(name, description, numFloors, monsters, boss, locationClass)
        {
        }

        public List<string> GetMonstersOnFloor(int floor)
        {
            return base._floorMonsters[floor];
        }
    }
}

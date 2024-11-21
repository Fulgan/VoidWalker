using Spectre.Console;
using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.Game;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner;

/// <summary>
/// Now this here, this is yer basic game. Keeps track of the current location, party, etc.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game : IGame
{
    private Location _currentLocation = null!;
    private bool _isRunning = true;
    private List<Character> _party = new();
    private Inventory _inventory = new();

    public static IGame Current { get; private set; }

    public Game()
    {
        Current = this;
    }

    public void SetLocation(Location location)
    {
        _currentLocation = location;
    }

    public void Run()
    {
        LoadGameOrStartNewGame();

        // Don't execute code if we stay in the same location, e.g. press enter or "help" - only execute code
        // if the location changed. Fixes a bug where spamming enter keeps adding the same location over and over ...
        Location? previousLocation = null;

        while (_isRunning)
        {
            if (previousLocation != _currentLocation)
            {
                CodeBehindRunner.ExecuteLocationCode(_currentLocation);
            }

            LocationDisplayer.ShowLocation(_currentLocation);
            var command = InputProcessor.PromptForAction(_currentLocation);
            previousLocation = _currentLocation;

            var result = command.Execute(this, _party);

            foreach (var message in result)
            {
                AnsiConsole.MarkupLine(message);
            }

            ApplyResultsIfBattle(command);
        }
    }

    private void ApplyResultsIfBattle(ICommand command)
    {
        var dungeon = _currentLocation as Dungeon;

        // Kinda a special case for battle commands
        if (command is IBattleCommand battleCommand)
        {
            if (battleCommand.IsVictory)
            {
                // Wipe out the dungeon floor's inhabitants.
                dungeon.OnVictory(_inventory);
            }
            else
            {
                foreach (var character in _party)
                {
                    character.CurrentHealth = 1;
                }
            }
            
            var dungeonSaveData = new Dictionary<string, object>
            {
                { "CurrentFloor", dungeon.CurrentFloorNumber },
                { "IsClear", battleCommand.IsVictory }
            };
            SaveGameManager.SaveGame("default", _currentLocation.LocationId, _party, _inventory, dungeonSaveData);
        }
    }

    private void LoadGameOrStartNewGame()
    {
        if (SaveGameManager.HasSave("default"))
        {
            var data = SaveGameManager.LoadGame("default");
            _party = data.Party;
            GameSwitches.Switches = data.Switches;
            new ChangeLocationCommand(data.CurrentLocationId).Execute(this, _party);
            UnpackLocationSpecificdata(data);
        }
        else
        {
            var runner = new NewGameRunner(this);
            runner.ShowGameIntro();
            _party = runner.CreateParty();

            var startLocationId = runner.GetStartingLocationId();
            new ChangeLocationCommand(startLocationId).Execute(this, _party);
        }
    }

    private void UnpackLocationSpecificdata(SaveData data)
    {
        var extraData = data.LocationSpecificData;
        if (extraData == null || extraData.Count == 0)
        {
            return;
        }

        // Duck typing...
        var dungeon = _currentLocation as Dungeon;

        if (extraData.ContainsKey("CurrentFloor"))
        {
            var floorNumber = Convert.ToInt32(extraData["CurrentFloor"]);
            var isClear = (bool)extraData["IsClear"];

            dungeon.SetState(floorNumber, isClear);
        }
    }
}

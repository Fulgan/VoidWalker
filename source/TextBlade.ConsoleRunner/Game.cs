using Spectre.Console;
using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Battle;
using TextBlade.Core.Commands;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.Game;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;
using TextBlade.Core.Services;

namespace TextBlade.ConsoleRunner;


/// <summary>
/// Your basic game class. Keeps track of the current location, party members, etc. in save data.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game : IGame
{
    public static IGame Current { get; private set; } = null!;
    private const int AutoSaveIntervalMinutes = 1;

    public Inventory Inventory => _saveData.Inventory;
    
    private SaveData _saveData;

    private Location _currentLocation = null!;
    private readonly bool _isRunning = true;
    private DateTime _lastSaveOn = DateTime.Now;

    private readonly ISoundPlayer _backgroundAudioPlayer; 

    public Game(ISoundPlayer soundPlayer)
    {
        _backgroundAudioPlayer = soundPlayer;
        Current = this;
    }

    /// <summary>
    /// Called whenever a location changes. Sleeping in an inn, descending a dungeon, do NOT trigger this.
    /// </summary>
    public void SetLocation(Location location)
    {
        _currentLocation = location;
        _currentLocation.CurrentSaveData = _saveData;
        PlayBackgroundAudio();
        AutoSaveIfItsBeenAWhile();
    }

    public void Run()
    {
        try {
            LoadGameOrStartNewGame();

            // Don't execute code if we stay in the same location, e.g. press enter or "help" - only execute code
            // if the location changed. Fixes a bug where spamming enter keeps adding the same location over and over ...
            Location? previousLocation = null;

            while (_isRunning)
            {
                if (previousLocation != _currentLocation)
                {
                    CodeBehindRunner.ExecuteLocationCode(_currentLocation);
                    LocationDisplayer.ShowLocation(_currentLocation);
                }

                var command = InputProcessor.PromptForAction(_currentLocation);
                previousLocation = _currentLocation;

                var messages = command.Execute(this, _saveData.Party);
                foreach (string message in messages)
                {
                    AnsiConsole.MarkupLine(message);
                }

                /// This area stinks: type-specific things...
                var dungeonSaveData = BattleResultsApplier.ApplyResultsIfBattle(command, _currentLocation, _saveData);
                if (command is IBattleCommand)
                {
                    SaveGame(dungeonSaveData);

                    // After battle, tell me the floor status again.
                    LocationDisplayer.ShowLocation(_currentLocation);
                }
                else if (command is ManuallySaveCommand)
                {
                    SaveGame(dungeonSaveData);
                }
                else if (command is LookCommand)
                {
                    LocationDisplayer.ShowLocation(_currentLocation);
                }
                else
                {
                    AutoSaveIfItsBeenAWhile(dungeonSaveData);
                }
            }
        }
        catch (Exception ex)
        {
            string[] crashFiles = [@"SaveData\default.save", "crash.txt"];
            AnsiConsole.MarkupLine("[red]Oh no! The game crashed![/]");
            AnsiConsole.MarkupLine("Please reach out to the developers and let them know about this, so that they can look into it.");
            AnsiConsole.MarkupLine($"Send them these files from your game directory, along with a description of what you were doing in-game: [green]{string.Join(", ", crashFiles)}[/]");
            File.WriteAllText("crash.txt", ex.ToString());
        }
    }

    private void SaveGame(Dictionary<string, object>? locationSpecificData = null)
    {
        SaveGameManager.SaveGame("default", _currentLocation.LocationId, _saveData.Party, _saveData.Inventory, _saveData.Gold, locationSpecificData);
        AnsiConsole.MarkupLine("[green]Game saved.[/]");
    }

    private void LoadGameOrStartNewGame()
    {
        if (SaveGameManager.HasSave("default"))
        {
            _saveData = SaveGameManager.LoadGame("default");
            GameSwitches.Switches = _saveData.Switches;
            var messages = new ChangeLocationCommand(_saveData.CurrentLocationId).Execute(this, _saveData.Party);
            foreach (var message in messages)
            {
                // ... There is no message ... needed for IAsyncEnumerable to work ... ?
            }
            UnpackLocationSpecificData();
            AnsiConsole.WriteLine("Save game loaded. For help, type \"help\"");
        }
        else
        {
            var runner = new NewGameRunner(this);
            runner.ShowGameIntro();
            _saveData = new();
            _saveData.Party = runner.CreateParty();
            _saveData.Inventory = new();

            var startLocationId = runner.GetStartingLocationId();
            var messages = new ChangeLocationCommand(startLocationId).Execute(this, _saveData.Party);
            foreach (var message in messages)
            {
                // ... There is no message ... needed for IAsyncEnumerable to work ... ?
            }
            AnsiConsole.WriteLine("New game started. For help, type \"help\"");
        }
    }

    private void UnpackLocationSpecificData()
    {
        var extraData = _saveData.LocationSpecificData;
        if (extraData == null || extraData.Count == 0)
        {
            return;
        }

        // Duck typing...
        var dungeon = _currentLocation as Dungeon;

        // These are all dungeon-specific
        if (dungeon == null)
        {
            return;
        }

        var floorNumber = Convert.ToInt32(extraData["CurrentFloor"]);
        var isClear = (bool)extraData["IsClear"];
        dungeon.SetState(floorNumber, isClear);
    }
    
    private async void PlayBackgroundAudio()
    {
        _backgroundAudioPlayer.Stop();
        if (string.IsNullOrWhiteSpace(_currentLocation.BackgroundAudio))
        {
            return;
        }

        _backgroundAudioPlayer.Load(Path.Join("Content", "Audio", _currentLocation.BackgroundAudio));
        _backgroundAudioPlayer.Play();
    }

    private void AutoSaveIfItsBeenAWhile(Dictionary<string, object>? locationSpecificData = null)
    {
        var elapsed = DateTime.UtcNow - _lastSaveOn;
        if (elapsed.TotalMinutes < AutoSaveIntervalMinutes)
        {
            return;
        }
        
        _lastSaveOn = DateTime.UtcNow;
        SaveGame(locationSpecificData);
    }
}

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
/// Now this here, this is yer basic game. Keeps track of the current location, party, etc.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game : IGame
{
    public static IGame Current { get; private set; } = null!;

    public Inventory Inventory => _saveData.Inventory;
    
    // Don't kill the messenger. I swear, it's bad enough this only works on Windows.
    private const string SupportedAudioExtension = "wav";
    private const int AutoSaveIntervalMinutes = 1;
    private SaveData _saveData;

    private Location _currentLocation = null!;
    private readonly bool _isRunning = true;
    private DateTime _lastSaveOn = DateTime.Now;

    private readonly ISoundPlayer _backgroundAudioPlayer; 

    public Game(ISoundPlayer soundPlayer)
    {
        _backgroundAudioPlayer = soundPlayer;
        Current = this;
        _backgroundAudioPlayer.LoadCompleted += (sender, args) => _backgroundAudioPlayer.PlayLooping();
    }

    /// <summary>
    /// Called whenever a location changes. Sleeping in an inn, descending a dungeon, do NOT trigger this.
    /// </summary>
    /// <param name="location"></param>
    public void SetLocation(Location location)
    {
        _currentLocation = location;
        _currentLocation.CurrentSaveData = _saveData;
        PlayBackgroundAudio();
        AutoSaveIfItsBeenAWhile();
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
            AutoSaveIfItsBeenAWhile(dungeonSaveData);

            if (command is ManuallySaveCommand)
            {
                SaveGame();
            }
            else if (command is LookCommand)
            {
                LocationDisplayer.ShowLocation(_currentLocation);
            }
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
            AnsiConsole.WriteLine("Save game loaded.");
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
            AnsiConsole.WriteLine("New game started.");
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
    
    private void PlayBackgroundAudio()
    {
        _backgroundAudioPlayer.Stop();
        if (string.IsNullOrWhiteSpace(_currentLocation.BackgroundAudio))
        {
            return;
        }

        _backgroundAudioPlayer.SoundLocation = Path.Join("Content", "Audio", $"{_currentLocation.BackgroundAudio}.{SupportedAudioExtension}");
        _backgroundAudioPlayer.Load();
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

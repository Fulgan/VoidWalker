using System.Media;
using System.Runtime.CompilerServices;
using Spectre.Console;
using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.Game;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;
using TextBlade.Core.Services;
using TextBlade.Plateform.Windows;

namespace TextBlade.ConsoleRunner;


/// <summary>
/// Now this here, this is yer basic game. Keeps track of the current location, party, etc.
/// Handles some basic parsing: showing output, reading input, and processing it (delegation).
/// </summary>
public class Game : IGame
{
    public static IGame Current { get; private set; } = null!;
    public Inventory Inventory => _inventory;

    // Don't kill the messenger. I swear, it's bad enough this only works on Windows.
    private const string SupportedAudioExtension = "wav";
    private const int AutoSaveIntervalMinutes = 1;

    private Location _currentLocation = null!;
    private readonly bool _isRunning = true;
    private List<Character> _party = new();
    private Inventory _inventory = new();
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
            }

            LocationDisplayer.ShowLocation(_currentLocation);
            var command = InputProcessor.PromptForAction(_currentLocation);
            previousLocation = _currentLocation;

            var messages = command.Execute(this, _party);
            foreach (string message in messages)
            {
                AnsiConsole.MarkupLine(message);
            }

            /// This area stinks: type-specific things...
            ApplyResultsIfBattle(command);
            if (command is ManuallySaveCommand)
            {
                SaveGame();
            }
        }
    }

    private void ApplyResultsIfBattle(ICommand command)
    {
        var dungeon = _currentLocation as Dungeon;

        // Kinda a special case for battle commands
        if (command is not IBattleCommand battleCommand)
        {
            return;
        }

        if (battleCommand.IsVictory)
        {
            // Wipe out the dungeon floor's inhabitants.
            dungeon?.OnVictory(_inventory);
        }
        else
        {
            foreach (var character in _party)
            {
                character.Revive();
            }
        }
        
        var dungeonSaveData = new Dictionary<string, object>
        {
            { "CurrentFloor", dungeon?.CurrentFloorNumber??0 },
            { "IsClear", battleCommand.IsVictory }
        };

        SaveGame(dungeonSaveData);
    }

    private void SaveGame(Dictionary<string, object>? locationSpecificData = null)
    {
        SaveGameManager.SaveGame("default", _currentLocation.LocationId, _party, _inventory, locationSpecificData);
        AnsiConsole.MarkupLine("[green]Game saved.[/]");
    }

    private void LoadGameOrStartNewGame()
    {
        if (SaveGameManager.HasSave("default"))
        {
            var data = SaveGameManager.LoadGame("default");
            _party = data.Party;
            _inventory = data.Inventory;
            GameSwitches.Switches = data.Switches;
            var messages = new ChangeLocationCommand(data.CurrentLocationId).Execute(this, _party);
            foreach (var message in messages)
            {
                // ... There is no message ... needed for IAsyncEnumerable to work ... ?
            }
            UnpackLocationSpecificData(data);
            AnsiConsole.WriteLine("Save game loaded.");
        }
        else
        {
            var runner = new NewGameRunner(this);
            runner.ShowGameIntro();
            _party = runner.CreateParty();

            var startLocationId = runner.GetStartingLocationId();
            var messages = new ChangeLocationCommand(startLocationId).Execute(this, _party);
            foreach (var message in messages)
            {
                // ... There is no message ... needed for IAsyncEnumerable to work ... ?
            }
            AnsiConsole.WriteLine("New game started.");
        }
    }

    private void UnpackLocationSpecificData(SaveData data)
    {
        var extraData = data.LocationSpecificData;
        if (extraData == null || extraData.Count == 0)
            return;

        // Duck typing...
        var dungeon = _currentLocation as Dungeon;

        if (!extraData.TryGetValue("CurrentFloor", out var value)) 
            return;
        var floorNumber = Convert.ToInt32(value);
        var isClear = (bool)extraData["IsClear"];

        dungeon?.SetState(floorNumber, isClear);
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

    private void AutoSaveIfItsBeenAWhile()
    {
        var elapsed = DateTime.Now - _lastSaveOn;
        if (elapsed.TotalMinutes < AutoSaveIntervalMinutes)
            return;
        _lastSaveOn = DateTime.Now;
        SaveGame();
    }
}

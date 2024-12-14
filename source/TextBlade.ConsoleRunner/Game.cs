using NAudio.Wave.SampleProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    private DateTime _lastSaveOn = DateTime.UtcNow;

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
        // Only the freshest data here: rehydrate your current location state.
        // This is for when you leave dungeon, go to town, then back to the dungeon.
        // Fixes a bug where: clear a dungeon floor, go to town, go back to the dungeon real quick.
        // You saved, yes, but t_saveData is stale.
        if (SaveGameManager.HasSave("default"))
        {
            _saveData = SaveGameManager.LoadGame("default");
        }
        
        Location.CurrentSaveData = _saveData;

        if (location.LocationId == _saveData.CurrentLocationId)
        {
            location.SetStateBasedOnCustomSaveData(_saveData.LocationSpecificData);
        }
        
        // If a current location requires saving when you change away from it, save it here.
        if (_currentLocation?.GetCustomSaveData() != null)
        {
            SaveGame();
        }

        _currentLocation = location;
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
                BattleResultsApplier.ApplyResultsIfBattle(command, _currentLocation, _saveData);
                if (command is IBattleCommand)
                {
                    SaveGame();

                    // After battle, tell me the floor status again.
                    LocationDisplayer.ShowLocation(_currentLocation);
                }
                else if (command is ManuallySaveCommand)
                {
                    SaveGame();
                }
                else if (command is LookCommand)
                {
                    LocationDisplayer.ShowLocation(_currentLocation);
                }
                else
                {
                    AutoSaveIfItsBeenAWhile();
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

    private void SaveGame()
    {
        SaveGameManager.SaveGame("default", _currentLocation.LocationId, _saveData.Party, _saveData.Inventory, _saveData.Gold, _currentLocation.LocationId, _currentLocation.GetCustomSaveData());
        AnsiConsole.MarkupLine("[green]Game saved.[/]");
    }

    private void LoadGameOrStartNewGame()
    {
        var gameJson = ShowGameIntro();

        if (SaveGameManager.HasSave("default"))
        {
            _saveData = SaveGameManager.LoadGame("default");
            GameSwitches.Switches = _saveData.Switches;
            var messages = new ChangeLocationCommand(_saveData.CurrentLocationId).Execute(this, _saveData.Party);

            foreach (var message in messages)
            {
                // ... There is no message ... needed for IAsyncEnumerable to work ... ?
            }

            if (_saveData.LocationSpecificDataLocationId == _currentLocation.LocationId)
            {
                _currentLocation.SetStateBasedOnCustomSaveData(_saveData.LocationSpecificData);
            }

            AnsiConsole.WriteLine("Save game loaded. For help, type \"help\"");
        }
        else
        {
            var runner = new NewGameRunner(gameJson);
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

    private JObject ShowGameIntro()
    {
        var gameJsonPath = Path.Join("Content", "game.json");

        if (!File.Exists(gameJsonPath))
        {
            throw new InvalidOperationException("Content/game.json file is missing!");
        }

        AnsiConsole.Background = Color.Black;
        var gameJsonContents = File.ReadAllText(gameJsonPath);
        if (JsonConvert.DeserializeObject(gameJsonContents) is not JObject gameJson)
        {
            throw new Exception("game.json is not a valid JSON object!");
        }

        var version = File.ReadAllText("version.txt").Trim();
        var gameName = gameJson["GameName"];
        AnsiConsole.MarkupLine($"[white]Welcome to[/] [red]{gameName}[/] version [white]{version}[/]!");

        // Keeps things DRY
        return gameJson;
    }

    private void PlayBackgroundAudio()
    {
        _backgroundAudioPlayer.Stop();
        if (string.IsNullOrWhiteSpace(_currentLocation.BackgroundAudio))
        {
            return;
        }

        _backgroundAudioPlayer.Load(Path.Join("Content", "Audio", _currentLocation.BackgroundAudio));
        _backgroundAudioPlayer.Play();
    }

    private void AutoSaveIfItsBeenAWhile()
    {
        var elapsed = DateTime.UtcNow - _lastSaveOn;
        if (elapsed.TotalMinutes < AutoSaveIntervalMinutes)
        {
            return;
        }
        
        _lastSaveOn = DateTime.UtcNow;
        SaveGame();
    }
}

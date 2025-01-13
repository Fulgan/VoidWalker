using TextBlade.Core.Audio;
using TextBlade.Core.Commands;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.Game;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner.IO;

public class InputProcessor
{
    private readonly IConsole _console;
    private readonly ISerialSoundPlayer _serialSoundPlayer;
    private readonly ISoundPlayer _soundPlayer;
    private readonly IGame _game;

    public InputProcessor(IGame game, IConsole console, ISerialSoundPlayer serialSoundPlayer, ISoundPlayer soundPlayer)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(serialSoundPlayer);
        ArgumentNullException.ThrowIfNull(soundPlayer);

        _game = game;
        _console = console;
        _serialSoundPlayer = serialSoundPlayer;
        _soundPlayer = soundPlayer;
    }

    public ICommand PromptForAction(Location currentLocation)
    {
        _console.WriteLine("Enter a command, or the number of your destination.");
        var rawResponse = _console.ReadLine();
        
        // It's some special command that the location handles. That doesn't change location.
        var command = currentLocation.GetCommandFor(rawResponse);
        
        // This is necessary ONLY for Dungeon, again, because there's no way to tell the player that they can't descend etc.
        var extraOutput = currentLocation.GetExtraOutputFor(rawResponse);
        if (!string.IsNullOrWhiteSpace(extraOutput))
        {
            _console.WriteLine(extraOutput);
        }

        // TODO: this probably shouldn't go here. I don't know how else to do this.
        // Weird corner case for FightCommand, which requires a TurnBasedCombatSystem, which requires lots of dependencies
        if (command is FightCommand fight)
        {
            var dungeon = currentLocation as Dungeon;
            if (dungeon is null)
            {
                throw new InvalidOperationException("Can't set up a fight system without a dungeon thus far");
            }

            var currentFloorData = dungeon.GetCurrentFloorData();
            var loot = dungeon.GetCurrentFloorLoot();
            var system = new TurnBasedBattleSystem(_console, _serialSoundPlayer, _soundPlayer, dungeon, currentFloorData, loot);
            fight.System = system;
        }

        if (!(command is DoNothingCommand))
        {
            return command;
        }

        // No? Maybe it's a destination?
        int destinationOption;
        if (int.TryParse(rawResponse, out destinationOption))
        {
            // Check if it's valid
            if (destinationOption <= 0 || destinationOption > currentLocation.LinkedLocations.Count)
            {
                _console.WriteLine("That's not a valid destination!");
                return new DoNothingCommand();
            }

            var destination = currentLocation.LinkedLocations[destinationOption - 1];
            return new ChangeLocationCommand(_game, destination.Id);
        }

        // Nah, nah, it's not a destination, just a global command.
        // If you update this, update the help listing in ShowHelpCommand.
        switch (rawResponse)
        {
            case "quit":
            case "q":
                return new QuitGameCommand();
            case "i":
            case "inv":
            case "inventory":
                return new ShowInventoryCommand(_soundPlayer);
            case "p":
            case "party":
            case "status":
                return new ShowPartyStatusCommand();
            case "credits":
                return new ShowCreditsCommand();
            case "s":
            case "save":
                return new ManuallySaveCommand();
            case "l":
            case "look":
                return new LookCommand();
            case "t":
            case "talk":
                return new TalkCommand();
            case "help":
            case "h":
            case "?":
                return new ShowHelpCommand();
        }

        return new DoNothingCommand();
    }
}

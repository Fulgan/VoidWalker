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
        var command = currentLocation.GetCommandFor(_console, _serialSoundPlayer, _soundPlayer, rawResponse);

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
                return new QuitGameCommand(_console);
            case "i":
            case "inv":
            case "inventory":
                return new ShowInventoryCommand(_console, _soundPlayer);
            case "p":
            case "party":
            case "status":
                return new ShowPartyStatusCommand(_console);
            case "credits":
                return new ShowCreditsCommand(_console);
            case "s":
            case "save":
                return new ManuallySaveCommand();
            case "l":
            case "look":
                return new LookCommand();
            case "help":
            case "h":
            case "?":
                return new ShowHelpCommand(_console);
        }

        return new DoNothingCommand();
    }
}

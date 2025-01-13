using TextBlade.Core.Game;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class ChangeLocationCommand : ICommand
{
    // Used for saving, so we know the ID of our current location
    private readonly string _locationId;
    private readonly string _locationPath;
    private readonly IGame _game;

    public ChangeLocationCommand(IGame game, string destinationId)
    {
        ArgumentNullException.ThrowIfNull(game);

        var locationFileName = destinationId.ToString().Replace('/', Path.DirectorySeparatorChar);
        var locationPath = Path.Join("Content", $"{locationFileName}.json");
        if (!File.Exists(locationPath))
        {
            throw new InvalidOperationException($"{locationPath} doesn't seem to exist!");
        }

        _game = game;
        _locationId = destinationId;
        _locationPath = locationPath;
    }

    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
    {
        var locationData = Serializer.Deserialize<Location>(File.ReadAllText(_locationPath));
        locationData.LocationId = _locationId;
        _game.SetLocation(locationData);
        return true;
    }
}

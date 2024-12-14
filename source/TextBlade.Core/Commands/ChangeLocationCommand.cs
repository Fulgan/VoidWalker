using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class ChangeLocationCommand : ICommand
{
    // Used for saving, so we know the ID of our current location
    private readonly string _locationId;
    private readonly string _locationPath;

    public ChangeLocationCommand(string destinationId)
    {
        var locationFileName = destinationId.ToString().Replace('/', Path.DirectorySeparatorChar);
        var locationPath = Path.Join("Content", $"{locationFileName}.json");
        if (!File.Exists(locationPath))
        {
            throw new InvalidOperationException($"{locationPath} doesn't seem to exist!");
        }

        _locationId = destinationId;
        _locationPath = locationPath;
    }

    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        var locationData = Serializer.Deserialize<Location>(File.ReadAllText(_locationPath));
        locationData.LocationId = _locationId;
        game.SetLocation(locationData);
        yield return string.Empty;
    }
}

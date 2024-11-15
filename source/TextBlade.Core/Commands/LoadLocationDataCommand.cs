using TextBlade.Core.Characters;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class LoadLocationDataCommand : Command
{
    private readonly string _destinationId;
    
    public LoadLocationDataCommand(string destinationId)
    {
        _destinationId = destinationId;
    }

    public override Location? Execute(List<Character> party)
    {
        var locationFileName = _destinationId.ToString().Replace('/', Path.DirectorySeparatorChar);
        var locationPath = Path.Join("Content", "Locations", $"{locationFileName}.json");
        if (!File.Exists(locationPath))
        {
            throw new InvalidOperationException($"{locationPath} doesn't seem to exist!");
        }

        var locationData = Serializer.Deserialize<Location>(File.ReadAllText(locationPath));
        return locationData;
    }
}

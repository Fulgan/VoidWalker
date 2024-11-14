using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class ChangeLocationCommand : Command
{
    public string DestinationId { get; private set; }
    
    public ChangeLocationCommand(string destinationId)
    {
        this.DestinationId = destinationId;
    }

    public override Location Execute()
    {
        var locationName = DestinationId.ToString().Replace('/', Path.DirectorySeparatorChar);
        var locationPath = Path.Join("Content", "Locations", $"{locationName}.json");
        if (!File.Exists(locationPath))
        {
            throw new InvalidOperationException($"{locationPath} doesn't seem to exist!");
        }

        var locationData = Serializer.Deserialize<Location>(File.ReadAllText(locationPath));
        return locationData;
    }
}

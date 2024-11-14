using TextBlade.Core.Locations;

namespace TextBlade.ConsoleRunner;

public static class LocationDisplayer
{
    /// <summary>
    /// Show a location. Ask for a destination. Then, return the link. Raw data.
    /// </summary>
    public static LocationLink ShowLocation(Location currentLocation)
    {
        ShowLocationAndLinkedLocations(currentLocation);
        var destinationId = PromptForDestination(currentLocation);
        return destinationId;
    }

    private static void ShowLocationAndLinkedLocations(Location currentLocation)
    {
        if (currentLocation == null)
        {
            throw new InvalidOperationException("Current location is null!");
        }

        Console.WriteLine($"You are in {currentLocation.Name}: {currentLocation.Description}");
        Console.WriteLine($"You can go to {currentLocation.LinkedLocations.Count} places:");
        
        int i = 0;
        foreach (var location in currentLocation.LinkedLocations)
        {
            i++;
            Console.WriteLine($"    {i}: {location.Description}");
        }
    }

    private static LocationLink PromptForDestination(Location currentLocation)
    {
        Console.Write("Enter the number of your destination: ");
        var answer = int.Parse(Console.ReadLine().Trim());
        // Assume it's valid
        var destination = currentLocation.LinkedLocations[answer - 1];
        return destination;
    }
}

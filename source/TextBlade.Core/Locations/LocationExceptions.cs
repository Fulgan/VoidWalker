namespace TextBlade.Core.Locations;

public static class LocationExceptions
{
    public static void ThrowIfNot<T>(this Location location) where T : Location
    {
        if (!(location is T))
        {
            // To lift this restriction, as of writing, make an interface with OnVictory and add it to Dungeon.
            // Then, this method can take in any IHasOnVictory class instance.
            throw new ArgumentException($"Location isn't a {typeof(T).Name}.");
        }
    }
}

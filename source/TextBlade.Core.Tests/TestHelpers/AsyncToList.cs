namespace TextBlade.Core.Tests.TestHelpers;

public static class AsyncToList
{
    public static async Task<List<string>> ToList(IAsyncEnumerable<string> strings)
    {
        var toReturn = new List<string>();
        await foreach (var s in strings)
        {
            toReturn.Add(s);
        }
        return toReturn;
    }
}

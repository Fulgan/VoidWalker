namespace TextBlade.Core.Collections;

/// <summary>
/// A utility class to get a weighted random item.
/// Adapted from: https://gamedev.stackexchange.com/questions/162976/how-do-i-create-a-weighted-collection-and-then-pick-a-random-element-from-it
/// </summary>
class WeightedRandomBag<T>
{
    private struct Entry
    {
        public double AccumulatedWeight;
        public T Item;
    }

    private List<Entry> _entries = new List<Entry>();
    private double _accumulatedWeight;

    public WeightedRandomBag(IDictionary<T, double> itemProbabilities)
    {
        foreach (var kvp in itemProbabilities)
        {
            this.AddEntry(kvp.Key, kvp.Value);
        }
    }

    public T GetRandom()
    {
        double r = Random.Shared.NextDouble() * _accumulatedWeight;

        foreach (Entry entry in _entries)
        {
            if (entry.AccumulatedWeight >= r)
            {
                return entry.Item;
            }
        }
        
        return default(T); // should only happen when there are no entries
    }

    private void AddEntry(T item, double weight)
    {
        _accumulatedWeight += weight;
        _entries.Add(new Entry { Item = item, AccumulatedWeight = _accumulatedWeight });
    }
}


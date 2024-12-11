var stones = File
    .ReadLines("input")
    .First().Split(' ')
    .GroupBy(l => l)
    .ToDictionary(d => d.Key, d => (long)d.Count());

// Store a dictionary of number on stones, and a count of how many stones there are.
// Process each stone number as a batch, instead of repeating calculations.
for (var i = 0; i < 75; i++)
{
    var stonesTemp = new Dictionary<string, long>(stones); // Change temp instead of the dictionary iterating over
    blink(stones, stonesTemp);
    stones = new Dictionary<string, long>(stonesTemp);
    if (i == 24) Console.WriteLine($"Part One: {stones.Sum(s => s.Value)}");
}

Console.WriteLine($"Part Two: {stones.Sum(s => s.Value)}");

return;

void blink(IEnumerable<KeyValuePair<string, long>> beforeBlinkStones, Dictionary<string, long> afterBlinkStones)
{
    foreach (var stone in beforeBlinkStones)
    {
        if (stone.Value == 0) continue; // Ignore values that no stones are set to.

        afterBlinkStones[stone.Key] -= stone.Value; // After blinking all these stones will be gone, remove them

        if (stone.Key == "0") // Rule one
        {
            addOrIncrementStones("1", stone.Value, afterBlinkStones);
            continue;
        }

        if (int.IsEvenInteger(stone.Key.Length)) // Rule two
        {
            var left = stone.Key[..(stone.Key.Length / 2)];
            var right = stone.Key[(stone.Key.Length / 2)..];
            addOrIncrementStones(left, stone.Value, afterBlinkStones);
            // Parse the right side to remove leading '0's. 
            addOrIncrementStones(long.Parse(right).ToString(), stone.Value, afterBlinkStones);
            continue;
        }

        addOrIncrementStones((long.Parse(stone.Key) * 2024).ToString(), stone.Value, afterBlinkStones); // Rule three
    }
}

void addOrIncrementStones(string key, long value, Dictionary<string, long> stonesToChange)
{
    if (!stonesToChange.TryAdd(key, value)) // Creates a default value if the dict doesn't contain one yet
    {
        stonesToChange[key] += value;
    }
}
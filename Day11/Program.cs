var stones = File
    .ReadLines("input")
    .First().Split(' ')
    .GroupBy(l => l)
    .ToDictionary(d => d.Key, d => (long)d.Count());

var newStones = new Dictionary<string, long>(stones);

long partOne = 0;

// Store a dictionary of numbers, and a count of their numbers.
// Process each number as a batch, instead of repeating calculations
// Don't separately work out "1" for 5000+ different "0"s, and all other values.
for (var i = 0; i < 75; i++)
{
    blink(stones, newStones);
    stones = new Dictionary<string, long>(newStones);
    if (i == 24) partOne = stones.Sum(s => s.Value);
}

Console.WriteLine($"Part One: {partOne}");
Console.WriteLine($"Part Two: {stones.Sum(s => s.Value)}");

return;

void blink(IEnumerable<KeyValuePair<string, long>> stonesToCheck, Dictionary<string, long> newStonesList)
{
    foreach (var stone in stonesToCheck)
    {
        if (stone.Value == 0) continue;

        removeStones(stone.Key, stone.Value, newStonesList);

        if (stone.Key == "0")
        {
            addOrIncrementStones("1", stone.Value, newStonesList);
            continue;
        }

        if (int.IsEvenInteger(stone.Key.Length))
        {
            var left = stone.Key[..(stone.Key.Length / 2)];
            var right = stone.Key[(stone.Key.Length / 2)..];
            addOrIncrementStones(long.Parse(right).ToString(), stone.Value, newStonesList);
            addOrIncrementStones(long.Parse(left).ToString(), stone.Value, newStonesList);
            continue;
        }

        addOrIncrementStones((long.Parse(stone.Key) * 2024).ToString(), stone.Value, newStonesList);
    }
}

void removeStones(string key, long value, Dictionary<string, long> stonesToChange)
{
    stonesToChange[key] -= value;
    stonesToChange[key] = long.Max(stonesToChange[key], 0);
}

void addOrIncrementStones(string key, long value, Dictionary<string, long> stonesToChange)
{
    if (!stonesToChange.TryAdd(key, value))
    {
        stonesToChange[key] += value;
    }
}
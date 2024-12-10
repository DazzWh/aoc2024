var input = File.ReadAllLines("input")
    .Select(l => l.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();

var trailheads = new Queue<(int, int)>();

for (var y = 0; y < input.Length; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == 0) // Add all 0s to a trailheads queue
        {
            trailheads.Enqueue(new(x, y));
        }
    }
}

Console.WriteLine($"Part One: {trailheads.Sum(t => calculateTrailheadScore(t))}");
Console.WriteLine($"Part Two: {trailheads.Sum(t => calculateTrailheadScore(t, true))}");

return;

int calculateTrailheadScore((int x, int y) trailhead, bool countAllPaths = false)
{
    var score = 0;
    var nines = new HashSet<(int, int)>();
    var paths = new Queue<(int, int)>([trailhead]);
    var walkableDirections = new[] { (X: -1, Y: 0), (X: 1, Y: 0), (X: 0, Y: -1), (X: 0, Y: 1) };

    while (paths.Count > 0)
    {
        var (x, y) = paths.Dequeue();

        if (input[y][x] == 9) // Reached end of a path
        {
            if (!nines.Contains((x, y))) score++;
            if (!countAllPaths) nines.Add((x, y)); // Part 2, just turn off adding the distinct nines check.
            continue;
        }

        // Enqueue all paths around position that are within bounds, and equal +1 to current path value
        foreach (var direction in walkableDirections.Where(d => d.X + x >= 0 && d.X + x < input[0].Length &&
                                                                d.Y + y >= 0 && d.Y + y < input[0].Length &&
                                                                input[y + d.Y][x + d.X] == input[y][x] + 1))
        {   
            paths.Enqueue((direction.X + x, direction.Y + y));
        }
    }

    return score;
}
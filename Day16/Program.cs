var input = File.ReadAllLines("input").Select(l => l.ToCharArray()).ToArray();

var start = (X: 0, Y: 0);

for (var y = 0; y < input.Length; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == 'S') start = (x, y);
    }
}

/*
 * This solution was done whilst juggling lots of other things.
 * It represents the messiness of the week and why I got behind.
 * I am not cleaning it up as I now consider it art, a working solution of mess.
 */

var paths = new SortedList<int, List<Path>>();
paths.Add(input[0].Length * start.Y + start.X, [new Path(start, (X: 1, Y: 0), [start])]);
var minScore = 160624L;
var walkableDirections = new[] { (X: -1, Y: 0), (X: 1, Y: 0), (X: 0, Y: -1), (X: 0, Y: 1) };

var minScoreAtLocation = Enumerable.Repeat(long.MaxValue, input.Length * input[0].Length).ToArray();

var smallestWalks = new List<Path>();

while (paths.Count > 0)
{
    var path = paths.First().Value.First();
    paths.First().Value.RemoveAt(0);

    if (!paths.First().Value.Any())
    {
        paths.RemoveAt(0);
    }

    if (path.Score > minScore) continue;
    var (x, y) = path.Pos;

    foreach (var direction in walkableDirections.Where(d => d.X + x >= 0 && d.X + x < input[0].Length &&
                                                            d.Y + y >= 0 && d.Y + y < input.Length))
    {
        var checkPos = (X: direction.X + x, Y: direction.Y + y);

        if (direction == (path.Facing.X * -1, path.Facing.Y * -1) ||
            input[checkPos.Y][checkPos.X] == '#' ||
            path.WalkedPositions.Contains(checkPos))
        {
            continue;
        }

        var newPathScore = path.Score;
        if (direction == (path.Facing.X, path.Facing.Y))
        {
            newPathScore += 1;
        }
        else
        {
            newPathScore += 1001;
        }

        if (input[checkPos.Y][checkPos.X] == 'E' && newPathScore <= minScore)
        {
            minScore = newPathScore;

            var finalWalked = new HashSet<(int X, int Y)>(path.WalkedPositions);
            finalWalked.Add(checkPos);
            smallestWalks.Add(new Path(checkPos, direction, finalWalked, minScore));
            continue;
        }

        if (minScoreAtLocation[input[0].Length * checkPos.Y + checkPos.X + input[0].Length] < newPathScore)
        {
            continue; // Only carry on with minimum score from x,y
        }
        minScoreAtLocation[input[0].Length * checkPos.Y + checkPos.X + input[0].Length] = newPathScore + 1001;

        var newWalkedPositions = new HashSet<(int X, int Y)>(path.WalkedPositions);
        newWalkedPositions.Add(checkPos);

        if (paths.ContainsKey(input[0].Length * checkPos.Y + checkPos.X + input[0].Length))
        {
            paths[input[0].Length * checkPos.Y + checkPos.X + input[0].Length]
                .Add(new Path(checkPos, direction, newWalkedPositions, newPathScore));
        }
        else
        {
            paths.Add(input[0].Length * checkPos.Y + checkPos.X + input[0].Length,
                [new Path(checkPos, direction, newWalkedPositions, newPathScore)]);
        }
    }
}
var walkedPaths = new HashSet<(int X, int Y)>();
foreach (var path in smallestWalks.Where(p => p.Score == minScore))
{
    foreach (var pos in path.WalkedPositions)
    {
        walkedPaths.Add((pos.X, pos.Y));
    }
}

printMap((-1, -1), walkedPaths);
Console.WriteLine($"Part One: {minScore}");
Console.WriteLine($"Part Two: {walkedPaths.Count}");
return;

void printMap((int x, int y) pos, HashSet<(int X, int Y)> map)
{
    Console.WriteLine("---------------------");
    for (var y = 0; y < input.Length; y++)
    {
        Console.WriteLine();
        for (var x = 0; x < input[y].Length; x++)
        {
            //if (pos == (x, y)) Console.Write("O");
            if (map.Contains((x, y))) Console.Write('O');
            else Console.Write(input[y][x]);
        }
    }

    Console.WriteLine();
    Console.WriteLine();
}

public record Path((int X, int Y) Pos, (int X, int Y) Facing, HashSet<(int X, int Y)> WalkedPositions, long Score = 0);
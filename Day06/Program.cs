var input = File.ReadAllLines("input").Select(s => s.ToCharArray()).ToArray();
var startPos = getGuardPosition(input);

var walkedPath = getWalkedPath(input, startPos, null).WalkedPath;
var partOne = walkedPath.Sum(l => l.Count(c => c == 'X'));

var partTwo = Enumerable.Range(0, input.Length).Select(y => Enumerable.Range(0, input[y].Length)
    .Count(x => walkedPath[y][x] == 'X' && getWalkedPath(walkedPath, startPos, (X: x, Y: y)).IsLoop)).Sum();

Console.WriteLine($"Part one: {partOne}");
Console.WriteLine($"Part two: {partTwo}");

return;

(char[][] WalkedPath, bool IsLoop) getWalkedPath(
    char[][] input,
    (int X, int Y) startPosition,
    (int X, int Y)? blockPosition)
{
    var walkedPath = input.Select(l => l.ToArray()).ToArray();

    if (blockPosition.HasValue)
    {
        walkedPath[blockPosition.Value.Y][blockPosition.Value.X] = '#';
    }

    (int X, int Y)[] directions =
    [
        (X: 0, Y: -1), (X: 1, Y: 0), (X: 0, Y: 1), (X: -1, Y: 0)
    ];

    var di = 0;
    var guardPosition = startPosition;
    var visited = new HashSet<(int D, int X, int Y)>();

    while (positionIsInBounds(guardPosition))
    {
        if (visited.Contains((di, guardPosition.X, guardPosition.Y)))
        {
            return (walkedPath, true);
        }

        walkedPath[guardPosition.Y][guardPosition.X] = 'X';
        visited.Add((di, guardPosition.X, guardPosition.Y));

        while (positionIsInBounds((X: guardPosition.X + directions[di].X, Y: guardPosition.Y + directions[di].Y)) &&
               walkedPath[guardPosition.Y + directions[di].Y][guardPosition.X + directions[di].X] == '#')
        {
            di = (di + 1) % directions.Length;
        }

        guardPosition.X += directions[di].X;
        guardPosition.Y += directions[di].Y;
    }

    return (walkedPath, false);
}

bool positionIsInBounds((int X, int Y) p)
{
    return p.X >= 0 && p.X < input.Length &&
           p.Y >= 0 && p.Y < input.Length;
}

(int X, int Y) getGuardPosition(char[][] input)
{
    var y = Array.FindIndex(input, l => l.Contains('^'));
    var x = Array.IndexOf(input[y], '^');
    return (x, y);
}
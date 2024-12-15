var mapStrings = File.ReadAllLines("input").Where(line => line.StartsWith('#')).ToArray();
var moveStrings = File.ReadAllLines("input").Where(line => !line.StartsWith('#'));

var walls = new HashSet<(int x, int y)>();
var boxes = new HashSet<(int x, int y)>();
var robot = (x: 0, y: 0);
var dirs = new Dictionary<char, (int x, int y)>
{
    { '^', (0, -1) },
    { '>', (1, 0) },
    { 'v', (0, 1) },
    { '<', (-1, 0) },
};

for (var y = 0; y < mapStrings.Length; y++)
{
    for (var x = 0; x < mapStrings.First().Length; x++)
    {
        switch (mapStrings[y][x])
        {
            case '#':
            {
                walls.Add((x * 2, y));
                walls.Add((x * 2 + 1, y));
                break;
            }
            case 'O':
            {
                boxes.Add((x * 2, y));
                break;
            }
            case '@':
            {
                robot = (x * 2, y);
                break;
            }
        }
    }
}

foreach (var move in string.Join("", moveStrings))
{
    if (walls.Contains((robot.x + dirs[move].x, robot.y + dirs[move].y))) continue; // Don't move into walls.

    if (!boxAtPosition((robot.x + dirs[move].x, robot.y + dirs[move].y), out var firstBox)) // Move into empty space
    {
        robot = (robot.x + dirs[move].x, robot.y + dirs[move].y);
        continue;
    }

    var boxCheckDirs = new Dictionary<char, (int x, int y)[]>
    {
        { '^', [(0, -1), (1, -1)] },
        { '>', [(2, 0)] },
        { 'v', [(0, 1), (1, 1)] },
        { '<', [(-1, 0)] },
    };

    var boxesToCheck = new Queue<(int x, int y)>();
    boxesToCheck.Enqueue(firstBox);
    var pushedBoxes = new HashSet<(int x, int y)> { firstBox };
    var canPush = true;
    while (boxesToCheck.Count > 0 && canPush)
    {
        var boxPos = boxesToCheck.Dequeue();

        foreach (var boxCheckPos in boxCheckDirs[move])
        {
            if (boxAtPosition((boxPos.x + boxCheckPos.x, boxPos.y + boxCheckPos.y), out var hitBox))
            {
                if (!pushedBoxes.Contains(hitBox))
                {
                    boxesToCheck.Enqueue(hitBox);
                    pushedBoxes.Add(hitBox);
                }
            }

            if (walls.Contains((boxPos.x + boxCheckPos.x, boxPos.y + boxCheckPos.y)))
            {
                canPush = false;
            }
        }
    }

    if (canPush)
    {
        foreach (var box in pushedBoxes.Reverse())
        {
            boxes.Remove(box);
            boxes.Add((box.x + dirs[move].x, box.y + dirs[move].y));
        }

        robot = (robot.x + dirs[move].x, robot.y + dirs[move].y); // Move robot into that space.
    }
}

Console.WriteLine($"Part two: {boxes.Sum(b => 100 * b.y + b.x)}");
return;

bool boxAtPosition((int x, int y) pos, out (int x, int y) boxPosition)
{
    boxPosition = boxes.FirstOrDefault(b => (b.x == pos.x && b.y == pos.y) || (b.x + 1 == pos.x && b.y == pos.y));
    return boxPosition != default;
}
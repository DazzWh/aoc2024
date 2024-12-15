namespace Day15;

public static class PartOne
{
    public static void Run(string inputFile)
    {
        var mapStrings = File.ReadAllLines(inputFile).Where(line => line.StartsWith('#'));
        var moveStrings = File.ReadAllLines(inputFile).Where(line => !line.StartsWith('#'));

        // Felt like trying this one without a 2d array at all, which is my usual approach.
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

        foreach (var line in mapStrings.Index())
        {
            foreach (var thing in line.Item.Index())
            {
                switch (thing.Item)
                {
                    case '#': walls.Add((thing.Index, line.Index)); break;
                    case 'O': boxes.Add((thing.Index, line.Index)); break;
                    case '@': robot = (thing.Index, line.Index); break;
                }
            }
        }

        foreach (var move in string.Join("", moveStrings))
        {
            var dir = dirs[move];
            if (walls.Contains((robot.x + dir.x, robot.y + dir.y))) return; // Don't move into walls.

            if (!boxes.Contains((robot.x + dir.x, robot.y + dir.y))) // Move into empty space
            {
                robot = (robot.x + dir.x, robot.y + dir.y);
                return;
            }
    
            var checkPos = (x: robot.x + dir.x, y: robot.y + dir.y);
            while (boxes.Contains(checkPos)) // Get thing behind box that isn't a box.
            {
                checkPos = (checkPos.x + dir.x, checkPos.y + dir.y);
            }

            if (walls.Contains(checkPos)) return; // It's a wall, can't push, carry on.

            boxes.Add(checkPos); // It's empty space, put a box here.
            boxes.Remove((robot.x + dir.x, robot.y + dir.y)); // Remove first box.
            robot = (robot.x + dir.x, robot.y + dir.y); // Move robot into that space.
        }

        Console.WriteLine($"Part 1: {boxes.Sum(b => 100 * b.y + b.x)}");
    }
}
var input = File.ReadAllLines("input")
    .Select(l => $"...{l}...") // Add padding to stop out of bounds errors
    .ToList();
input.InsertRange(0, Enumerable.Repeat(new string('.', input[0].Length), 3)); // Top padding
input.AddRange(Enumerable.Repeat(new string('.', input[0].Length), 3)); // Bot padding

var partOne = 0;
var partTwo = 0;

for (var y = 0; y < input.Count; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == 'X')
        {
            partOne += countXmasWordsAtPosition(y, x);
        }

        if (input[y][x] == 'A' && xmasCrossIsAtPosition(y, x))
        {
            partTwo++;
        }
    }
}

Console.WriteLine($"Part One: {partOne}");
Console.WriteLine($"Part Two: {partTwo}");

return;

int countXmasWordsAtPosition(int y, int x)
{
    var count = 0;

    (int X, int Y)[] directions =
    [
        (X: -1, Y: 0), (X: 1, Y: 0), (X: 0, Y: -1), (X: 0, Y: 1), // Left, Right, Up, Down
        (X: 1, Y: -1), (X: 1, Y: 1), (X: -1, Y: 1), (X: -1, Y: -1) // Diagonals
    ];

    foreach (var dir in directions)
    {
        var currentPos = (X: x, Y: y);

        // Check for M, then A, then S while "moving" in dir
        if ("MAS".All(c => input[currentPos.Y += dir.Y][currentPos.X += dir.X] == c))
        {
            count++;
        }
    }

    return count;
}

bool xmasCrossIsAtPosition(int y, int x)
{
    foreach (var c in ((int X, int Y)[][])
             [ // These positions need to contain both S and M, when starting on an A, to create an X-MAS cross
                 [(X: 1, Y: -1), (X: -1, Y: 1)], // UpRight/DownLeft
                 [(X: -1, Y: -1), (X: 1, Y: 1)] // UpLeft/DownRight
             ])
    {
        char[] charsAtCheckPositions = [input[c[0].Y + y][c[0].X + x], input[c[1].Y + y][c[1].X + x]];
        if (!"SM".All(charsAtCheckPositions.Contains))
        {
            return false; // The diagonally checked positions don't contain both S & M, so this is not a cross
        }
    }

    return true;
}
var input = File.ReadAllLines("input");
Point.SetMax(input[0].Length, input.Length);

var frequencies = new Dictionary<char, List<Point>>();

for (var y = 0; y < input.Length; y++)
{
    for (var x = 0; x < input[0].Length; x++)
    {
        if (input[y][x] != '.')
        {
            if (frequencies.TryGetValue(input[y][x], out var points))
            {
                points.Add(new Point(x, y));
            }
            else
            {
                frequencies.Add(input[y][x], [new Point(x, y)]);
            }
        }
    }
}

var antinodes = new HashSet<Point>();

foreach (var freq in frequencies)
{
    foreach (var antenna in freq.Value)
    {
        foreach (var otherAntenna in freq.Value.Where(p => !Equals(p, antenna)))
        {
            var possibleNodePoint =  antenna + antenna - otherAntenna;
            if (possibleNodePoint.IsInBounds() )
            {
                antinodes.Add(possibleNodePoint);
            }
        }
    }
}

var partOne = antinodes.Count;

Console.WriteLine($"Part One: {partOne}");

class Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    private static int[] Max = [];
    public static void SetMax(int x, int y) => Max = [x, y];
    public bool IsInBounds() => X < Max[0] && X >= 0 && Y < Max[1] && Y >= 0;
    public static Point operator -(Point p1, Point p2) => new(p1.X - p2.X, p1.Y - p2.Y);
    public static Point operator +(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
    public override bool Equals(object? obj) => obj is Point p && X == p.X && Y == p.Y;
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";
}
var input = File.ReadAllLines("input")
    .Select(l => $"..{l}..") // Add padding to stop out of bounds errors
    .ToList();
input.InsertRange(0, Enumerable.Repeat(new string('.', input[0].Length), 2)); // Top padding
input.AddRange(Enumerable.Repeat(new string('.', input[0].Length), 2)); // Bot padding

var map = input.Select(l => l.ToCharArray()).ToArray();
var mapChecked = new bool[input.Count].Select(_ => new bool[input[0].Length]).ToArray();
var regions = new List<Region>();

for (var y = 0; y < input.Count; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        if (mapChecked[y][x] || map[y][x] == '.') continue;
        regions.Add(getRegion(new(x, y)));
    }
}

Console.WriteLine($"Part One:{regions.Sum(r => r.FullPrice)}");
Console.WriteLine($"Part Two:{regions.Sum(r => r.DiscountPrice(map))}");

return;

Region getRegion((int x, int y) start)
{
    var region = new Region();
    var plotsToCheck = new Queue<(int, int)>([start]);
    while (plotsToCheck.Count > 0)
    {
        var (x, y) = plotsToCheck.Dequeue();
        if (mapChecked[y][x]) continue;

        mapChecked[y][x] = true;
        region.Area++;
        region.PlotPoints.Add(new(x, y));

        foreach (var dir in new[] { (X: -1, Y: 0), (X: 1, Y: 0), (X: 0, Y: -1), (X: 0, Y: 1) })
        {
            var checkPos = (X: dir.X + x, Y: dir.Y + y);

            if (map[checkPos.Y][checkPos.X] != map[y][x])
            {
                region.Perimeter++; // If not the same char, count as perimeter
            }

            if (map[checkPos.Y][checkPos.X] == map[y][x] && mapChecked[checkPos.Y][checkPos.X] == false)
            {
                plotsToCheck.Enqueue(checkPos); // If unchecked and the same char, add to queue
            }
        }
    }

    return region;
}

class Region
{
    public int Area;
    public int Perimeter;
    public List<(int X, int Y)> PlotPoints = new();
    public int FullPrice => Area * Perimeter; // Part One
    public int DiscountPrice(char[][] map) // Part Two, amount of sides is same as corners
    {
        var corners = 0;

        var cornerChecks = new Dictionary<(int X, int Y), (int X, int Y)[]>();
        cornerChecks.Add((-1, -1), [(0, -1), (-1, 0)]); // Top Left, Up & Left
        cornerChecks.Add((-1, 1), [(0, 1), (-1, 0)]); // Bot Left, Down & Left
        cornerChecks.Add((1, -1), [(0, -1), (1, 0)]); // Top Right, Up & Right
        cornerChecks.Add((1, 1), [(0, 1), (1, 0)]); // Bot Right, Down & Right

        foreach (var plotPoint in PlotPoints)
        {
            foreach (var cornerCheck in cornerChecks)
            {
                var regionChar = map[PlotPoints.First().Y][PlotPoints.First().X];
                var cornerChar = map[plotPoint.Y + cornerCheck.Key.Y][plotPoint.X + cornerCheck.Key.X];
                var sideOneChar = map[plotPoint.Y + cornerCheck.Value[0].Y][plotPoint.X + cornerCheck.Value[0].X];
                var sideTwoChar = map[plotPoint.Y + cornerCheck.Value[1].Y][plotPoint.X + cornerCheck.Value[1].X];

                if ( (regionChar != sideOneChar && regionChar != sideTwoChar) || // Outside corners
                     (regionChar == sideOneChar && regionChar == sideTwoChar && regionChar != cornerChar)) // Inside corners
                {
                    corners++;
                }
            }
        }

        return corners * Area;
    }
}
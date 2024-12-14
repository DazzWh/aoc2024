using System.Text.RegularExpressions;

var input = File.ReadAllLines("input")
    .Select(l => new Regex(@"([\d]*),([\d]*) v=(-?[\d]*),(-?[\d]*)").Matches(l))
    .Select(mc => mc.Select(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value))).First().ToArray())
    .Select(m => new
    {
        P = (X: m[0], Y: m[1]),
        V = (X: m[2], Y: m[3])
    }).ToArray();

var (maxX, maxY) = (101, 103);
var quadrantsCount = new[] { 0, 0, 0, 0 };

foreach (var robot in input)
{
    var endPosition = (
        X: modWithNegatives(robot.P.X + robot.V.X * 100, maxX),
        Y: modWithNegatives(robot.P.Y + robot.V.Y * 100, maxY));

    if (endPosition.X < maxX / 2 && endPosition.Y < maxY / 2) quadrantsCount[0]++;
    if (endPosition.X > maxX / 2 && endPosition.Y < maxY / 2) quadrantsCount[1]++;
    if (endPosition.X < maxX / 2 && endPosition.Y > maxY / 2) quadrantsCount[2]++;
    if (endPosition.X > maxX / 2 && endPosition.Y > maxY / 2) quadrantsCount[3]++;
}

Console.WriteLine($"Part One: {quadrantsCount.Aggregate(1, (a, b) => a * b)}");

var partTwo = 0;
while (true) // Part two is "when do the robots become a picture of a Christmas tree"
{
    var map = new char[maxY][]; // Make blank map
    for (var y = 0; y < maxY; y++) map[y] = new string('.', maxX).ToCharArray();
    
    foreach (var robot in input) map[robot.P.Y][robot.P.X] = '0'; // Draw robots on map
    foreach (var line in map) Console.WriteLine(string.Join("", line)); // Print map

    Console.WriteLine($"\nNumber of iterations: {partTwo} \nIs it a tree yet? ");
    if (Console.ReadLine() == "Yes, I am done you can stop now, thank you.") break;

    for (var i = 0; i < input.Length; i++)
    {
        input[i] = new
        {
            P = (X: modWithNegatives(input[i].P.X + input[i].V.X, maxX),
                Y: modWithNegatives(input[i].P.Y + input[i].V.Y, maxY)),
            V = (input[i].V.X, input[i].V.Y),
        };
    }

    partTwo++;
}

Console.WriteLine($"Congratulations, Part Two: {partTwo}");
return;

int modWithNegatives(int v, int m)
{
    return (v % m + m) % m;
}
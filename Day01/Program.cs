using MoreLinq;

var listOne = new List<int>();
var listTwo = new List<int>();

File.ReadAllLines("input")
    .Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    .ForEach(s =>
    {
        listOne.Add(int.Parse(s[0]));
        listTwo.Add(int.Parse(s[1]));
    });

var partOne = listOne.OrderDescending().Zip(listTwo.OrderDescending(),
    (first, second) => Math.Abs(first - second)).Sum();

var partTwo = listOne.Sum(i => i * listTwo.Count(x => x == i));

Console.WriteLine($"PartOne: {partOne}");
Console.WriteLine($"PartTwo: {partTwo}");
var input = File.ReadAllLines("input").First().ToList();

var diskMap = new List<char>();

for (var i = 0; i < input.Count; i++)
{
    var id = i > 0 ? i / 2 : 0;
    var block = i == 0 || int.IsEvenInteger(i) ? id.ToString().First() : '.';
    diskMap.AddRange(Enumerable.Repeat(block, int.Parse(input[i].ToString())).ToList());
}

var left = diskMap.FindIndex(0, c => c.Equals('.'));
var right = diskMap.FindLastIndex(diskMap.Count - 1, c => !c.Equals('.'));

while (left < right)
{
    (diskMap[left], diskMap[right]) = (diskMap[right], diskMap[left]);
    left = diskMap.FindIndex(left, c => c.Equals('.'));
    right = diskMap.FindLastIndex(right, c => !c.Equals('.'));
}

long partOne = 0;

for (var i = 0; i <= diskMap.FindLastIndex(diskMap.Count - 1, c => !c.Equals('.')); i++)
{
    partOne += i * int.Parse(diskMap[i].ToString());
}

Console.WriteLine($"Part One: {partOne}");

// 5937137598 is too low

// IDs can be multiple numbers, need to store i[0] as 12 for example.
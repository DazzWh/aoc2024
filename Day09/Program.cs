var input = File.ReadAllLines("input").First().ToList();

var diskMap = new List<int>();

for (var i = 0; i < input.Count; i++)
{
    var id = i > 0 ? i / 2 : 0;
    var block = i == 0 || int.IsEvenInteger(i) ? id : -1;
    var amount = int.Parse(input[i].ToString());
    diskMap.AddRange(Enumerable.Repeat(block, amount).ToList());
}

var left = diskMap.FindIndex(0, c => c == -1);
var right = diskMap.FindLastIndex(diskMap.Count - 1, c => c != -1);
while (left < right)
{
    (diskMap[left], diskMap[right]) = (diskMap[right], diskMap[left]);
    left = diskMap.FindIndex(left, c => c == -1);
    right = diskMap.FindLastIndex(right, c => c != -1);
}

long partOne = 0;
for (var i = 0; i < diskMap.Count(d => d >= 0); i++)
{
    partOne += i * diskMap[i];
}

Console.WriteLine($"Part One: {partOne}");
var input = File.ReadAllLines("input").First().ToList();

var diskMap = new List<DiskChunk>();

for (var i = 0; i < input.Count; i++)
{
    var id = i > 0 ? i / 2 : 0;
    diskMap.Add(new DiskChunk(int.Parse(input[i].ToString()), int.IsEvenInteger(i) ? id : -1));
}

for (var i = diskMap.Max(c => c.Value); i > 0; i--)
{
    var chunkToMoveIdx = diskMap.FindIndex(c => c.Value == i);
    var bigEnoughSpaceIdx = diskMap.FindIndex(0, c => c.IsFreeSpace && c.Size >= diskMap[chunkToMoveIdx].Size);
    if (bigEnoughSpaceIdx != -1 && bigEnoughSpaceIdx < chunkToMoveIdx)
    {
        var chunk = diskMap[chunkToMoveIdx];
        diskMap.Remove(chunk);
        diskMap.Insert(bigEnoughSpaceIdx, chunk);
        diskMap[bigEnoughSpaceIdx + 1].Size -= chunk.Size;
        diskMap.Insert(chunkToMoveIdx, new DiskChunk(chunk.Size, -1));
    }
}

long partTwo = 0;
var idx = 0;
foreach (var chunk in diskMap)
{
    if (chunk.IsFreeSpace)
    {
        idx += chunk.Size;
    }
    else
    {
        for (var i = 0; i < chunk.Size; i++)
        {
            partTwo += idx * chunk.Value;
            idx++;
        }
    }
}

Console.WriteLine($"Part two: {partTwo}");

internal class DiskChunk(int size, int val)
{
    public int Size = size;
    public readonly int Value = val;
    public bool IsFreeSpace => Value == -1;
}
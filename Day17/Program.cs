using System.Collections.Concurrent;

var input = File.ReadAllLines("input");

var regA = long.Parse(input[0].Split(':').Last().Trim()); // operand 4
var regB = long.Parse(input[1].Split(':').Last().Trim()); // operand 5
var regC = long.Parse(input[2].Split(':').Last().Trim()); // operand 6
var program = input[4].Split(':').Last().Split(',').Select(long.Parse).ToArray();

var computer = new Computer(regA, regB, regC, program);
computer.RunProgram();
Console.WriteLine($"Part One: {string.Join(",", computer.Output)}");

var partTwo = new ConcurrentBag<long>(); // Brute force, cleaning my house today...
var buffer = 0;
const int ChunkMax = 9999999;
while (partTwo.IsEmpty)
{
    Parallel.ForEach(Enumerable.Range(0, ChunkMax), (i, j) =>
    {
        var testA = buffer > 0 ? buffer * ChunkMax + i : i;
        var testCpu = new Computer(testA, regB, regC, program, true);
        testCpu.RunProgram();

        if (!testCpu.Output.SequenceEqual(program)) return;

        partTwo.Add(testA);
        Console.WriteLine($"Part Two Working Value: {testA}");
    });

    buffer++;
    Console.WriteLine($"Buffered Times: {buffer}");
}

Console.WriteLine($"Part Two Smallest: {partTwo.Min()}");
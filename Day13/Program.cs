using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;

var input = File.ReadAllLines("input")
    .Where(l => !string.IsNullOrWhiteSpace(l))
    .Index()
    .GroupBy(x => x.Index / 3) // Get groups of 3 
    .Select(g => g.Select(x => x.Item))
    .Select(g => g.Select(s => s.Split(','))) // Split all lines by ,
    .Select(g => g.Select(l => l.Select(p => Regex.Replace(p, "[^0-9]", "")))) // Get only the digits
    .Select(g => g.Select(l => l.Select(int.Parse)).ToArray()) // Parse to int
    .Select(g => new // Create named button configuration objects from the values
    {
        ButtonA = (X: g[0].First(), Y: g[0].Last()),
        ButtonB = (X: g[1].First(), Y: g[1].Last()),
        Prize = (X: g[2].First(), Y: g[2].Last()),
    }).ToArray();


Console.WriteLine($"Part One: {input.Sum(m => getLowestCost(m.ButtonA, m.ButtonB, m.Prize, true))}");
Console.WriteLine($"Part Two: {input.Sum(m => getLowestCost(m.ButtonA, m.ButtonB, m.Prize))}");

return;

long getLowestCost(
    (int X, int Y) buttonA,
    (int X, int Y) buttonB,
    (int X, int Y) prize,
    bool partOne = false)
{
    // Solve simultaneous equation with matrices.
    var matrixA = Matrix<double>.Build.DenseOfArray(new double[,]
        {
            { buttonA.X, buttonB.X }, { buttonA.Y, buttonB.Y }
        });

    var matrixB = Vector<double>.Build.Dense
        ([
            partOne ? prize.X : prize.X + 10000000000000, // Part 2 adds 10000000000000 :)
            partOne ? prize.Y : prize.Y + 10000000000000
        ]);

    var solvedValues = matrixA.Solve(matrixB);
    var aButtonPresses = solvedValues[0];
    var bButtonPresses = solvedValues[1];

    if (partOne && (aButtonPresses > 100 || bButtonPresses > 100))
    {
        return 0; // Part one only allows 100 maximum button presses per button
    }

    return solvedValues // If values are whole numbers, there is a solution, otherwise return 0
        .All(n => Math.Abs(n - Convert.ToInt64(n)) < 0.0001)
        ? Convert.ToInt64(aButtonPresses * 3 + bButtonPresses)
        : 0; // A button presses * 3 + B button presses = cost.
}
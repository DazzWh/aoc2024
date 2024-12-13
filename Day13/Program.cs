using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;

var input = File.ReadAllLines("input")
    .Where(l => !string.IsNullOrWhiteSpace(l))
    .Index()
    .GroupBy(x => x.Index / 3) // Get groups of 3 
    .Select(g => g.Select(x => x.Item))
    .Select(g => g.Select(g => g.Split(','))) // Split all lines by ,
    .Select(g => g.Select(l => l.Select(p => Regex.Replace(p, "[^0-9]", "")))) // Get only the digits
    .Select(g => g.Select(l => l.Select(int.Parse)).ToArray()) // Parse to int
    .Select(g => new // Create named button configuration objects from the ints
    {
        A = (X: g[0].First(), Y: g[0].Last()),
        B = (X: g[1].First(), Y: g[1].Last()),
        Prize = (X: g[2].First(), Y: g[2].Last()),
    }).ToArray();


Console.WriteLine($"Part One: {input.Sum(m => getLowestCost(m.A, m.B, m.Prize, true))}");
Console.WriteLine($"Part Two: {input.Sum(m => getLowestCost(m.A, m.B, m.Prize))}");

return;

long getLowestCost((int X, int Y) A, (int X, int Y) B, (int X, int Y) Prize, bool partOne = false)
{
    // Solve simultaneous equation with matrices.
    var matrixA = Matrix<double>.Build.DenseOfArray(new double[,] { { A.X, B.X }, { A.Y, B.Y } });
    var matrixB = Vector<double>.Build.Dense
        ([partOne ? Prize.X : Prize.X + 10000000000000, partOne ? Prize.Y : Prize.Y + 10000000000000]);
    var solved = matrixA.Solve(matrixB);

    if (partOne && (solved[0] > 100 || solved[1] > 100))
    {
        return 0; // Part one only allows 100 maximum button presses
    }

    return solved // If values are whole numbers, there is a solution, otherwise return 0
        .All(n => Math.Abs(n - Convert.ToInt64(n)) < 0.0001) ? 
        Convert.ToInt64(solved[0] * 3 + solved[1]) : 0; // A button presses * 3 + B button presses = cost.
}
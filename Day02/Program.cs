var reports = File.ReadAllLines("input").Select(s => s.Split(' '));

var partOne = reports.Where(isSafe).Count();
var partTwo = reports.Where(isSafeWithDampener).Count();

Console.WriteLine($"Part one: {partOne}");
Console.WriteLine($"Part two: {partTwo}");

return;

bool isSafe(string[] report)
{
    var r = report.Select(int.Parse).ToList();
    var isIncreasing = r[0] < r[1];
    
    for (var i = 0; i < r.Count - 1; i++)
    {
        if(r[i] == r[i+1]) return false;
        if((isIncreasing && r[i] > r[i+1]) ||
           (!isIncreasing && r[i] < r[i+1] )) return false;
        if (Math.Abs(r[i] - r[i + 1]) > 3) return false;
    }

    return true;
}  

bool isSafeWithDampener(string[] report)
{
    if (isSafe(report)) return true;

    for (var idx = 0; idx < report.Length; idx++)
    {
        var reportWithoutIdx = report.ToList();
        reportWithoutIdx.RemoveAt(idx);
        if(isSafe(reportWithoutIdx.ToArray())) return true;
    }
    
    return false;
}
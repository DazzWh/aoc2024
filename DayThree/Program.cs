using System.Text.RegularExpressions;
using MoreLinq;

var input = File.ReadAllLines("input")
    .Aggregate((s, l) => s + Environment.NewLine + l);

var partOne = new Regex(@"mul\(\d*,\d*\)")
    .Matches(input)
    .Select(regexMatch => regexMatch.Value)
    .Select(getMulValueFromString)
    .Sum();

Console.WriteLine($"Part 1: {partOne}");

var partTwo = 0;
var doing = true;

new Regex(@"(mul\(\d*,\d*\))|(don\'t)|(do)")
    .Matches(input)
    .ForEach(match =>
    {
        if (match.Groups[1].Success && doing) partTwo += getMulValueFromString(match.Value);
        if (match.Groups[2].Success) doing = false;
        if (match.Groups[3].Success) doing = true;
    });

Console.WriteLine($"Part 2: {partTwo}");

return; 

int getMulValueFromString(string mulString)
{
    // multiplies values from strings formatted "mul(x,y)"
    var substring = mulString.Substring(4, mulString.Length - 5);
    return int.Parse(substring.Split(',')[0]) * int.Parse(substring.Split(',')[1]);
}
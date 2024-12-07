var input = File.ReadAllLines("input");

var partOne = input
    .Where(l => equationIsPossiblyTrue(l))
    .Sum(l => long.Parse(l.Split(':')[0]));

var partTwo = input
    .Where(l => equationIsPossiblyTrue(l, true))
    .Sum(l => long.Parse(l.Split(':')[0]));

Console.WriteLine($"Part one: {partOne}");
Console.WriteLine($"Part two: {partTwo}");

return;

bool equationIsPossiblyTrue(string equation, bool canConcat = false)
{
    var targetValue = long.Parse(equation.Split(':')[0]);
    var nums = equation.Split(": ")[1].Split(' ').Select(long.Parse).ToArray();

    foreach (var permutation in getOperatorPermutations(nums, canConcat))
    {
        var testValue = nums[0];

        for (var op = 0; op < permutation.Length; op++)
        {
            switch (permutation[op])
            {
                case '0': testValue += nums[op + 1]; break;
                case '1': testValue *= nums[op + 1]; break;
                case '2': testValue = long.Parse($"{testValue.ToString()}{nums[op + 1].ToString()}"); break;
            }
        }

        if (testValue == targetValue)
        {
            return true;
        }
    }

    return false;
}

IEnumerable<string> getOperatorPermutations(long[] nums, bool canConcat)
{
    if (!canConcat) // Part 1, generate strings of binary values. Eg max would be "1111" (for 5 numbers). 
    {               // Use these to determine permutations for each possible operator combination.
        return Enumerable
            .Range(0, Convert.ToInt32(new('1', nums.Count() - 1), 2) + 1)
            .Select(i => Convert.ToString(i, 2).PadLeft(nums.Count() - 1, '0'));
    }

    return Enumerable // Part 2, same as part 1 but in ternary (base 3) instead of binary. Eg. "222" (for 4 numbers)
        .Range(0, ternaryStringToInt(new string('2', nums.Length - 1)) + 1)
        .Select(i => intToTernaryString(i).PadLeft(nums.Length - 1, '0'));
}

// Base-n conversions should be built into C#...
int ternaryStringToInt(string ternaryString)
{
    var value = 0;
    for (var i = 0; i < ternaryString.Length; i++)
    {
        var n = int.Parse(ternaryString[i].ToString());
        var pow = ternaryString.Length - i - 1;

        value += n * (int)Math.Pow(3, pow);
    }

    return value;
}

string intToTernaryString(int decNumb)
{
    var ternaryString = string.Empty;
    do
    {
        ternaryString = decNumb % 3 + ternaryString;
        decNumb = decNumb / 3 | 0;
    } while (decNumb >= 1);

    return ternaryString;
}
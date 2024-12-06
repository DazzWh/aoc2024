var input = File.ReadAllLines("input");

var orderings = input
    .Where(l => l.Contains('|'))
    .Select(l => l.Split('|'))
    .Select(l => new[] { Convert.ToInt32(l[0]), Convert.ToInt32(l[1]) })
    .ToArray();

var updates = input
    .Where(l => l.Contains(','))
    .Select(l => l.Split(','))
    .Select(n => n.Select(int.Parse).ToArray())
    .ToArray();

var partOne = updates
    .Where(isInCorrectOrder)
    .Sum(update => update[update.Length / 2]);

var partTwo = updates
    .Where(update => !isInCorrectOrder(update))
    .Select(sortIntoCorrectOrder)
    .Sum(update => update[update.Length / 2]);

Console.WriteLine($"Part One: {partOne}");
Console.WriteLine($"Part Two: {partTwo}");

return;

bool isInCorrectOrder(int[] update)
{
    for (var i = 0; i < update.Length; i++)
    {
        foreach (var order in orderings.Where(o => o[0] == update[i]))
        {
            if (Array.IndexOf(update, order[1]) != -1 && Array.IndexOf(update, order[1]) < i)
            {
                return false;
            }
        }

        foreach (var order in orderings.Where(o => o[1] == update[i]))
        {
            if (Array.IndexOf(update, order[1]) != -1 && Array.IndexOf(update, order[1]) > i)
            {
                return false;
            }
        }
    }

    return true;
}

int[] sortIntoCorrectOrder(int[] update)
{
    var sorted = update.ToList();

    while (!isInCorrectOrder(sorted.ToArray()))
    {
        for (var i = 0; i < sorted.Count; i++)
        {
            foreach (var order in orderings.Where(o => o[0] == sorted[i]))
            {
                if (sorted.IndexOf(order[1]) != -1 && sorted.IndexOf(order[1]) < i)
                {
                    var value = sorted[i];
                    sorted.RemoveAt(i);
                    sorted.Insert(sorted.IndexOf(order[1]), value);
                }
            }

            foreach (var order in orderings.Where(o => o[1] == sorted[i]))
            {
                if (sorted.IndexOf(order[1]) != -1 && sorted.IndexOf(order[1]) > i)
                {
                    var value = sorted[i];
                    sorted.RemoveAt(i);
                    sorted.Insert(sorted.IndexOf(order[1] + 1), value);
                }
            }
        }
    }

    return sorted.ToArray();
}
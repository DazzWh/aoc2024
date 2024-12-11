var input = File.ReadLines("input").First().Split(' ').ToList();

Console.WriteLine($"Initial arrangement:\n{string.Join(' ', input)}\n");

for (int i = 0; i < 75; i++)
{
    blink(input);
    Console.WriteLine($"After {i+1} blink:\n{string.Join(' ', input)}\n");
}

Console.WriteLine($"Part One: {input.Count}");

return;

void blink(List<string> list)
{
    for (var i = 0; i < list.Count; i++)
    {
        if (list[i] == "0")
        {
            list[i] = "1";
            continue;
        }

        if (int.IsEvenInteger(list[i].Length))
        {
            var left = list[i][..(list[i].Length / 2)];
            var right = list[i][(list[i].Length / 2)..];
            list[i] = long.Parse(right).ToString();
            list.Insert(i, long.Parse(left).ToString());
            i++;
            continue;
        }

        list[i] = (long.Parse(list[i]) * 2024).ToString();
    }
}
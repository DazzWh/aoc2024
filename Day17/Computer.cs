public class Computer(long regA, long regB, long regC, long[] Program, bool sequenceEqualCheck = false)
{
    public long RegA = regA;
    public long RegB = regB;
    public long RegC = regC;
    public List<long> Output = new();

    public void RunProgram()
    {
        var instPtr = 0L;
        while (instPtr < Program.Length - 1)
        {
            var opcode = Program[instPtr];
            var literal = Program[instPtr + 1];
            var combo = literal switch
            {
                4 => RegA,
                5 => RegB,
                6 => RegC,
                _ => literal
            };

            switch (opcode)
            {
                case 0: // adv
                    RegA = (long)(RegA / Math.Pow(2, combo));
                    break;

                case 1: // bxl
                    RegB ^= literal;
                    break;

                case 2: // bst
                    RegB = combo % 8;
                    break;

                case 3: // jnz
                    if (RegA == 0) break;
                    instPtr = literal;
                    continue;

                case 4: // bxc
                    RegB ^= RegC;
                    break;

                case 5: // out
                    Output.Add(combo % 8);
                    if(sequenceEqualCheck && 
                       (Output.Count > Program.Length ||
                       !Output.SequenceEqual(Program[..Output.Count]))) return;
                    break;

                case 6: // bdv
                    RegB = (long)(RegA / Math.Pow(2, combo));
                    break;

                case 7: // cdv
                    RegC = (long)(RegA / Math.Pow(2, combo));
                    break;
            }

            instPtr += 2;
        }
    }
}
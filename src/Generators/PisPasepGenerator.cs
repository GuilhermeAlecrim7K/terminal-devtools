using System.Collections.Immutable;

namespace TerminalDevTools.Generators;

public static class PisPasepGenerator
{
    public static string PisPasep(this Random random)
    {
        int[] result = new int[11];
        for (int i = 0; i <= 9; i++)
            result[i] = random.Next(0, 10);

        var multipliers = ImmutableArray.Create([3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
        result[10] = 0;

        for (int i = 0; i < multipliers.Length; i++)
            result[10] += result[i] * multipliers[i];

        result[10] = 11 - (result[10] % 11);
        if (result[10] >= 10)
            result[10] = 0;

        return string.Join("", result.Select(i => $"{i}"));
    }
}
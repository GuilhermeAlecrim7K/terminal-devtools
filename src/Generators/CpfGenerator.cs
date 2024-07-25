namespace TerminalDevTools.Generators;

public static class CpfGenerator
{
    public static string Cpf(this Random random)
    {
        int[] result = new int[11];
        for (int i = 0; i <= 8; i++)
            result[i] = random.Next(0, 10);

        result[9] = 0;
        int factor = 10;
        for (int i = 0; i <= 8; i++)
            result[9] += result[i] * factor--;
        result[9] = result[9] % 11 < 2 ? 0 : 11 - (result[9] % 11);

        result[10] = 0;
        factor = 11;
        for (int i = 0; i <= 9; i++)
            result[10] += result[i] * factor--;
        result[10] = result[10] % 11 < 2 ? 0 : 11 - (result[10] % 11);

        return string.Join("", result.Select(d => $"{d}"));
    }
}
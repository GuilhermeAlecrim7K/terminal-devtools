namespace TerminalDevTools.Generators;

internal static class RgGenerator
{
    /// Model SSP-SP
    public static string Rg(this Random random)
    {
        int[] result = new int[8];
        for (int i = 0; i < result.Length; i++)
            result[i] = random.Next(0, 10);

        int multiplier = 9;
        int checkDigitMod = 0;
        for (int i = 0; i < result.Length; i++)
            checkDigitMod += result[i] * multiplier--;

        checkDigitMod %= 11;
        string checkDigit = checkDigitMod switch
        {
            10 => "X",
            11 => 0.ToString(),
            _ => checkDigitMod.ToString()
        };
        return string.Join("", result) + checkDigit;
    }
}
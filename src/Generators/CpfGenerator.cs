using System.Text.RegularExpressions;

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
    public static string Caepf(this Random random, string? cpf = null, string seq = "001")
    {
        if (cpf is not null && !Regex.IsMatch(input: cpf, pattern: @"^\d{9}\d{0,2}$"))
            throw new ArgumentException(message: null, paramName: nameof(cpf));
        if (seq is not null && !Regex.IsMatch(input: seq, pattern: @"^\d{3}$"))
            throw new ArgumentException(message: null, paramName: nameof(seq));
        cpf ??= random.Cpf();

        List<int> caepf = (cpf[0..9] + seq)
            .Select(c => int.Parse($"{c}"))
            .ToList();

        List<int> checkDigitMultipliers = [6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9];
        int checkDigitSum = 0;
        int checkDigit = 0;

        for (int i = 0; i < caepf.Count; i++)
            checkDigitSum += caepf[i] * checkDigitMultipliers[i];
        checkDigit = checkDigitSum % 11;
        if (checkDigit >= 10)
            checkDigit = 0;
        var checkDigits = checkDigit * 10;

        caepf.Add(checkDigit);
        checkDigitMultipliers.Insert(0, 5);

        checkDigitSum = 0;
        for (int i = 0; i < caepf.Count; i++)
            checkDigitSum += caepf[i] * checkDigitMultipliers[i];

        checkDigit = checkDigitSum % 11;
        if (checkDigit >= 10)
            checkDigit = 0;

        checkDigits += checkDigit + 12;
        if (checkDigits >= 100)
            checkDigits -= 100;

        caepf[12] = checkDigits / 10;
        caepf.Add(checkDigits % 10);

        return string.Join("", caepf);
    }
}
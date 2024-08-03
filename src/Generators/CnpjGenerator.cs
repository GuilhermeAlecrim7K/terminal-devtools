using System.Text.RegularExpressions;

namespace TerminalDevTools.Generators;

public static class CnpjGenerator
{

    public static string BaseCnpj(this Random random)
    {
        int[] root = new int[8];
        for (int i = 0; i < root.Length; i++)
            root[i] = random.Next(1, 10);
        return string.Join("", root);
    }

    public static string FullCnpj(this Random random, string? baseCnpj = null, string branch = "0001")
    {
        if (baseCnpj is not null && !Regex.IsMatch(input: baseCnpj, pattern: @"^\d{8}$"))
            throw new ArgumentException(message: null, paramName: nameof(baseCnpj));
        if (!Regex.IsMatch(input: branch, pattern: @"^\d{4}$"))
            throw new ArgumentException(message: null, paramName: nameof(branch));
        baseCnpj ??= random.BaseCnpj();

        List<int> branchCnpj = (baseCnpj + branch).ToCharArray().Select(c => int.Parse($"{c}")).ToList();

        List<int> checkDigitMultipliers = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int checkDigitSum = 0;
        int checkDigit = 0;

        for (int i = 0; i < branchCnpj.Count; i++)
            checkDigitSum += branchCnpj[i] * checkDigitMultipliers[i];
        checkDigit = 11 - (checkDigitSum % 11);
        if (checkDigit >= 10)
            checkDigit = 0;

        branchCnpj.Add(checkDigit);
        checkDigitMultipliers.Insert(0, 6);

        checkDigitSum = 0;
        for (int i = 0; i < branchCnpj.Count; i++)
            checkDigitSum += branchCnpj[i] * checkDigitMultipliers[i];

        checkDigit = 11 - (checkDigitSum % 11);
        if (checkDigit >= 10)
            checkDigit = 0;
        branchCnpj.Add(checkDigit);

        return string.Join("", branchCnpj);
    }
}
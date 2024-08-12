using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TerminalDevTools.Models;

public readonly record struct CnpjModel
{
    public readonly ImmutableArray<int> BaseNumber;
    public readonly ImmutableArray<int> Seq;
    public readonly int CheckDigit1;
    public readonly int CheckDigit2;
    public CnpjModel() : this(random: null) { }
    public CnpjModel(Random? random) : this(MakeRandomBaseNumber(random)) { }

    private static string MakeRandomBaseNumber(Random? random)
    {
        random ??= new Random();
        int[] result = new int[8];
        for (int i = 0; i < result.Length; i++)
            result[i] = random.Next(1, 10);
        return string.Join("", result) + "0001";
    }

    public CnpjModel(string baseCnpj)
    {
        ArgumentNullException.ThrowIfNull(baseCnpj, nameof(baseCnpj));
        var match = Regex.Match(baseCnpj, @"^(\d{8})(\d{4})$");
        if (!match.Success)
            throw new ArgumentException(message: null, paramName: nameof(baseCnpj));

        BaseNumber = [.. match.Groups[1].Value.ToCharArray().Select(c => int.Parse($"{c}"))];
        Seq = [.. match.Groups[2].Value.ToCharArray().Select(c => int.Parse($"{c}"))];
        CheckDigit1 = CalculateCheckDigit1();
        CheckDigit2 = CalculateCheckDigit2();
    }

    private int CalculateCheckDigit1()
    {
        int[] partialCnpj = [.. BaseNumber, .. Seq];

        List<int> multipliers = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < partialCnpj.Length; i++)
            sum += partialCnpj[i] * multipliers[i];
        int result = 11 - (sum % 11);
        if (result >= 10)
            result = 0;

        return result;
    }

    private int CalculateCheckDigit2()
    {
        int[] partialCnpj = [.. BaseNumber, .. Seq, CheckDigit1];

        List<int> multipliers = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < partialCnpj.Length; i++)
            sum += partialCnpj[i] * multipliers[i];
        int result = 11 - (sum % 11);
        if (result >= 10)
            result = 0;

        return result;
    }
    public string Formatted() => $"{string.Join("", BaseNumber[0..2])}.{string.Join("", BaseNumber[2..5])}.{string.Join("", BaseNumber[5..])}/{string.Join("", Seq)}-{CheckDigit1}{CheckDigit2}";
    public string Unformatted() => $"{string.Join("", BaseNumber.AddRange(Seq).Append(CheckDigit1).Append(CheckDigit2))}";
}
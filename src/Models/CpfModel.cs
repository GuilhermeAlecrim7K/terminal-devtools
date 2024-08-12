
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TerminalDevTools.Models;

public readonly record struct CpfModel
{
    public readonly ImmutableArray<int> BaseNumber;
    public readonly int CheckDigit1;
    public readonly int CheckDigit2;
    public CpfModel() : this(random: null) { }
    public CpfModel(Random? random) : this(MakeRandomBaseNumber(random)) { }

    private static string MakeRandomBaseNumber(Random? random)
    {
        random ??= new Random();
        int[] result = new int[9];
        for (int i = 0; i <= 8; i++)
            result[i] = random.Next(0, 10);
        return string.Join("", result);
    }

    public CpfModel(string baseCpf)
    {
        ArgumentNullException.ThrowIfNull(baseCpf, nameof(baseCpf));
        if (!Regex.IsMatch(baseCpf, @"^\d{9}$"))
            throw new ArgumentException(message: null, paramName: nameof(baseCpf));
        BaseNumber = [.. baseCpf.ToCharArray().Select(c => int.Parse($"{c}"))];
        CheckDigit1 = CalculateCheckDigit1();
        CheckDigit2 = CalculateCheckDigit2();
    }

    private int CalculateCheckDigit2()
    {
        var sum = 0;
        var factor = 11;
        for (int i = 0; i <= 8; i++)
            sum += BaseNumber[i] * factor--;
        sum += CheckDigit1 * factor;
        return sum % 11 < 2 ? 0 : 11 - (sum % 11);
    }

    private int CalculateCheckDigit1()
    {
        var sum = 0;
        int factor = 10;
        for (int i = 0; i <= 8; i++)
            sum += BaseNumber[i] * factor--;
        return sum % 11 < 2 ? 0 : 11 - (sum % 11);
    }

    public string Formatted() => $"{string.Join("", BaseNumber[0..3])}.{string.Join("", BaseNumber[3..6])}.{string.Join("", BaseNumber[6..])}-{CheckDigit1}{CheckDigit2}";
    public string Unformatted() => $"{string.Join("", BaseNumber)}{CheckDigit1}{CheckDigit2}";
}
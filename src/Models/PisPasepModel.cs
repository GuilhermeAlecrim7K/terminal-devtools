using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TerminalDevTools.Models;

public readonly record struct PisPasepModel
{
    public readonly ImmutableArray<int> BaseNumber;
    public readonly int CheckDigit;
    public PisPasepModel() : this(random: null) { }
    public PisPasepModel(Random? random) : this(MakeRandomBaseNumber(random)) { }

    private static string MakeRandomBaseNumber(Random? random)
    {
        random ??= new Random();
        int[] result = new int[10];
        for (int i = 0; i <= 9; i++)
            result[i] = random.Next(0, 10);
        return string.Join("", result);
    }

    public PisPasepModel(string basePisPasep)
    {
        ArgumentNullException.ThrowIfNull(basePisPasep, nameof(basePisPasep));
        if (!Regex.IsMatch(basePisPasep, @"\d{10}"))
            throw new ArgumentException(message: null, paramName: nameof(basePisPasep));

        BaseNumber = [.. basePisPasep.ToCharArray().Select(c => int.Parse($"{c}"))];
        CheckDigit = CalculateCheckDigit();

    }

    private int CalculateCheckDigit()
    {
        int[] multipliers = [3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < multipliers.Length; i++)
            sum += BaseNumber[i] * multipliers[i];

        int result = 11 - (sum % 11);
        if (result >= 10)
            result = 0;

        return result;
    }

    public string Formatted() => $"{string.Join("", BaseNumber[0..3])}.{string.Join("", BaseNumber[3..8])}.{string.Join("", BaseNumber[8..])}-{CheckDigit}";
    public string Unformatted() => $"{string.Join("", BaseNumber.Append(CheckDigit))}";
}
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TerminalDevTools.Models;

public readonly record struct RgSspSpModel
{
    public readonly ImmutableArray<int> BaseNumber;
    public readonly char CheckDigit;
    public RgSspSpModel() : this(random: null) { }
    public RgSspSpModel(Random? random) : this(MakeRandomBaseNumber(random)) { }

    private static string MakeRandomBaseNumber(Random? random)
    {
        random ??= new Random();
        int[] result = new int[8];
        for (int i = 0; i < result.Length; i++)
            result[i] = random.Next(0, 10);
        return string.Join("", result);
    }
    public RgSspSpModel(string baseRg)
    {
        ArgumentNullException.ThrowIfNull(baseRg, nameof(baseRg));
        if (!Regex.IsMatch(baseRg, @"\d{8}"))
            throw new ArgumentException(message: null, paramName: nameof(baseRg));
        BaseNumber = [.. baseRg.ToCharArray().Select(c => int.Parse($"{c}"))];
        CheckDigit = CalculateCheckDigit();
    }
    private char CalculateCheckDigit()
    {
        int multiplier = 9;
        int sum = 0;
        for (int i = 0; i < BaseNumber.Length; i++)
            sum += BaseNumber[i] * multiplier--;

        return (sum % 11) switch
        {
            int n when n < 10 => char.Parse($"{n}"),
            10 => 'X',
            11 => '0',
            _ => throw new Exception()
        };

    }

    public string Unformatted() => $"{string.Join("", BaseNumber)}{CheckDigit}";
}
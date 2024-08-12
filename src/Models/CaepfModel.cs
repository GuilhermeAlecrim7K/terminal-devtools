using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TerminalDevTools.Models;

public readonly record struct CaepfModel
{
    public readonly ImmutableArray<int> BaseNumber;
    public readonly ImmutableArray<int> Seq;
    public readonly int CheckDigit1;
    public readonly int CheckDigit2;
    private const string CaepfRegex = @"^(\d{9})(\d{3})$";
    public CaepfModel() : this(random: null) { }
    public CaepfModel(Random? random = null) : this(new CpfModel(random)) { }
    public CaepfModel(string caepfBase) : this(ExtractBaseCpfFromCaepfBase(caepfBase), ExtractSeqFromCaepfBase(caepfBase))
    { }

    private static CpfModel ExtractBaseCpfFromCaepfBase(string caepfBase)
    {
        var match = Regex.Match(caepfBase, CaepfRegex);
        return match.Success
            ? new CpfModel(baseCpf: match.Groups[1].Value)
            : throw new ArgumentException(message: null, paramName: nameof(caepfBase));
    }

    private static string ExtractSeqFromCaepfBase(string caepfBase)
    {
        var match = Regex.Match(caepfBase, CaepfRegex);
        return match.Success
            ? match.Groups[2].Value
            : throw new ArgumentException(message: null, paramName: nameof(caepfBase));
    }

    public CaepfModel(CpfModel baseCpf, string seq = "001")
    {
        ArgumentNullException.ThrowIfNull(baseCpf, nameof(baseCpf));
        ArgumentNullException.ThrowIfNull(seq, nameof(seq));
        if (!Regex.IsMatch(seq, @"^\d{3}$"))
            throw new ArgumentException(message: null, paramName: seq);

        Seq = [.. seq.ToCharArray().Select(c => int.Parse($"{c}"))];
        BaseNumber = baseCpf.BaseNumber;

        int partialCheckDigit1 = CalculatePartialCheckDigit1();
        int partialCheckDigit2 = CalculatePartialCheckDigit2(partialCheckDigit1);
        int partialCheckDigitSum = partialCheckDigit1 * 10 + partialCheckDigit2 + 12;
        if (partialCheckDigitSum >= 100)
            partialCheckDigitSum -= 100;

        CheckDigit1 = partialCheckDigitSum / 10;
        CheckDigit2 = partialCheckDigitSum % 10;
    }

    private int CalculatePartialCheckDigit1()
    {
        int[] multipliers = [6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9];
        int[] partialCaepf = [.. BaseNumber, .. Seq];
        int sum = 0;

        for (int i = 0; i < partialCaepf.Length; i++)
            sum += partialCaepf[i] * multipliers[i];
        int result = sum % 11;
        if (result >= 10)
            result = 0;
        return result;
    }

    private int CalculatePartialCheckDigit2(int partialCheckDigit1)
    {
        int[] multipliers = [5, 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9];
        int[] partialCaepf = [.. BaseNumber, .. Seq, partialCheckDigit1];
        int sum = 0;
        int result;

        for (int i = 0; i < partialCaepf.Length; i++)
            sum += partialCaepf[i] * multipliers[i];
        result = sum % 11;
        if (result >= 10)
            result = 0;
        return result;
    }
    public string Formatted() => $"{string.Join("", BaseNumber[0..3])}.{string.Join("", BaseNumber[3..6])}.{string.Join("", BaseNumber[6..])}/{string.Join("", Seq)}-{CheckDigit1}{CheckDigit2}";
    public string Unformatted() => $"{string.Join("", BaseNumber.AddRange(Seq).Append(CheckDigit1).Append(CheckDigit2))}";
}
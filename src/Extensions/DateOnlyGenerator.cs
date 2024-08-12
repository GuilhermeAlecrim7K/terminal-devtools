namespace TerminalDevTools.Extensions;

internal static class RandomDateExtension
{
    public static DateOnly NextDate(this Random random, DateOnly minValue, DateOnly maxValue)
    {
        var diff = maxValue.ToDateTime(TimeOnly.MinValue) - minValue.ToDateTime(TimeOnly.MinValue);
        return new DateOnly(minValue.Year, minValue.Month, minValue.Day)
            .AddDays(random.Next(diff.Days));
    }
}
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

using TerminalDevTools.Generators;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand
{
    private class GeneratePersonCommand : Command
    {
        private const string CpfOptionValueName = "cpf";

        private const string RgOptionValueName = "rg";
        private const string DateOfBirthOptionValueName = "date-of-birth";
        private const string PisOptionValueName = "pis";
        private static readonly string[] DataOptions = [
            RgOptionValueName,
            CpfOptionValueName,
            DateOfBirthOptionValueName,
            PisOptionValueName,
        ];
        private readonly Random _random = new();
        private readonly Option<IEnumerable<string>> _dataOption = new Option<IEnumerable<string>>
        (
            aliases: ["--data", "-d"],
            description: "A space separated list of values to be generated."
        )
        {
            AllowMultipleArgumentsPerToken = true,
        }.FromAmong(DataOptions);
        public GeneratePersonCommand() : base(name: "person", "Generates an individual's personal data")
        {
            AddOption(_dataOption);
            this.SetHandler(CommandHandler);
        }

        private void CommandHandler(InvocationContext context)
        {
            try
            {
                IEnumerable<string> data = context.ParseResult.GetValueForOption(_dataOption) ?? [];
                if (!data.Any())
                    data = data.Union(DataOptions);

                data = data.OrderBy(s => s);
                StringBuilder output = new();
                foreach (var option in data)
                {
                    output.AppendLine(option switch
                    {
                        DateOfBirthOptionValueName =>
                            $"date_of_birth={_random.NextDate(
                            DateOnly.FromDateTime(DateTime.Today).AddYears(-60),
                            DateOnly.FromDateTime(DateTime.Today).AddYears(-18)
                            ):yyyy-MM-dd}",
                        CpfOptionValueName => $"cpf={_random.Cpf()}",
                        PisOptionValueName => $"pis_pasep={_random.PisPasep()}",
                        RgOptionValueName => $"rg_ssp_sp={_random.Rg()}",
                        string any => throw new NotImplementedException($"Option {any} not implemented"),
                    });
                }
                context.Console.WriteLine(output.ToString());
                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                context.ExitCode = 1;
                context.Console.WriteLine(ex.ToString());
            }
        }
    }

}
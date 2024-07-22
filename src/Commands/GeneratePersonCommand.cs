using System.CommandLine;
using System.CommandLine.Invocation;

namespace TerminalDevTools.Commands;

public partial class GenerateCommand
{
    private class GeneratePersonCommand : Command
    {
        private const string FullNameOptionValueName = "full-name";
        private const string CpfOptionValueName = "cpf";

        private const string RgOptionValueName = "rg";
        private const string DateOfBirthOptionValueName = "date-of-birth";
        private const string PisOptionValueName = "pis";
        private const string GenderOptionValueName = "gender";
        private static readonly string[] DataOptions = [
            FullNameOptionValueName,
            RgOptionValueName,
            CpfOptionValueName,
            DateOfBirthOptionValueName,
            PisOptionValueName,
            GenderOptionValueName
        ];
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
                // TODO: implement here
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
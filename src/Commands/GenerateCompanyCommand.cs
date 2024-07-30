using System.CommandLine;
using System.CommandLine.Invocation;

namespace TerminalDevTools.Commands;

public partial class GenerateCommand
{
    private class GenerateCompanyCommand : Command
    {
        private const string BaseCnpjOptionValueName = "base-cnpj";
        private const string FullCnpjOptionValueName = "full-cnpj";
        private const string CpfOptionValueName = "cpf";
        private const string CaepfOptionValueName = "caepf";
        private static readonly string[] DataOptions = [
            BaseCnpjOptionValueName,
            FullCnpjOptionValueName,
            CpfOptionValueName,
            CaepfOptionValueName,
        ];
        private readonly Option<IEnumerable<string>> _dataOption = new Option<IEnumerable<string>>("data")
        {
            AllowMultipleArgumentsPerToken = true
        }.FromAmong(DataOptions);
        public GenerateCompanyCommand() : base(name: "company", "Generates a company's data")
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
                // TODO: implement generation here
                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                context.Console.WriteLine(ex.ToString());
                context.ExitCode = 1;
            }
        }
    }

}
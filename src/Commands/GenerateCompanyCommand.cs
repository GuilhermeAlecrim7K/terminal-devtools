using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

using TerminalDevTools.Generators;

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
        private readonly Random _random = new();

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
                data = data.OrderBy(s => s);
                StringBuilder output = new();
                string baseCnpj = _random.BaseCnpj();
                string cpf = _random.Cpf();
                foreach (var option in data)
                {
                    output.AppendLine(option switch
                    {
                        BaseCnpjOptionValueName =>
                            $"base_cnpj={baseCnpj}",
                        FullCnpjOptionValueName => $"full_cnpj={_random.FullCnpj(baseCnpj)}",
                        CpfOptionValueName => $"cpf={cpf}",
                        CaepfOptionValueName => $"caepf={_random.Caepf(cpf)}",
                        string any => throw new NotImplementedException($"Option {any} not implemented"),
                    });
                }
                context.Console.WriteLine(output.ToString());
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
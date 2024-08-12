using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

using TerminalDevTools.Models;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand
{
    private sealed class GenerateCompanyCommand : Command
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
        private readonly Option<bool> _formatOption;

        public GenerateCompanyCommand(Option<bool> formatOption) : base(name: "company", "Generates a company's data")
        {
            _formatOption = formatOption;
            AddOption(_dataOption);
            this.SetHandler(CommandHandler);
        }

        private void CommandHandler(InvocationContext context)
        {
            try
            {
                bool format = context.ParseResult.GetValueForOption(_formatOption);
                IEnumerable<string> data = context.ParseResult.GetValueForOption(_dataOption) ?? [];
                if (!data.Any())
                    data = data.Union(DataOptions);
                data = data.OrderBy(s => s);
                StringBuilder output = new();
                CnpjModel cnpj = new(_random);
                CpfModel cpf = new(_random);
                foreach (var option in data)
                {
                    output.AppendLine((option, format) switch
                    {
                        (BaseCnpjOptionValueName, _) =>
                            $"base_cnpj={string.Join("", cnpj.BaseNumber)}",
                        (FullCnpjOptionValueName, true) => $"full_cnpj={cnpj.Formatted()}",
                        (CpfOptionValueName, true) => $"cpf={cpf.Formatted()}",
                        (CaepfOptionValueName, true) => $"caepf={new CaepfModel(cpf).Formatted()}",
                        (FullCnpjOptionValueName, false) => $"full_cnpj={cnpj.Unformatted()}",
                        (CpfOptionValueName, false) => $"cpf={cpf.Unformatted()}",
                        (CaepfOptionValueName, false) => $"caepf={new CaepfModel(cpf).Unformatted()}",
                        (string any, _) => throw new NotImplementedException($"Option {any} not implemented"),
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
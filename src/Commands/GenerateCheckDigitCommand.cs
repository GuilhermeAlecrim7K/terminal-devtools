using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

using TerminalDevTools.Models;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand : Command
{
    private sealed class GenerateCheckDigitCommand : Command
    {
        private enum Algorithm
        {
            cpf,
            caepf,
            pis,
            cnpj,
            rg,
        }
        private readonly Option<Algorithm> _algorithmOption = new(["--algorithm", "-a"], "Specifies the target for the check digit algorithm.") { Arity = ArgumentArity.ExactlyOne, IsRequired = true };
        private readonly Argument<string> _inputArgument = new("input") { Arity = ArgumentArity.ExactlyOne };
        private readonly Random _random = new();
        private readonly Option<bool> _formatOption;

        public GenerateCheckDigitCommand(Option<bool> formatOption) : base("check-digit", "Generates check digits.")
        {
            _formatOption = formatOption;
            AddOption(_algorithmOption);
            AddArgument(_inputArgument);
            this.SetHandler(CommandHandler);
        }

        private void CommandHandler(InvocationContext context)
        {
            try
            {
                bool format = context.ParseResult.GetValueForOption(_formatOption);
                Algorithm algorithm = context.ParseResult.GetValueForOption(_algorithmOption);
                string input = context.ParseResult.GetValueForArgument(_inputArgument);
                context.Console.WriteLine((algorithm, format) switch
                {
                    (Algorithm.cpf, false) => new CpfModel(input).Unformatted(),
                    (Algorithm.caepf, false) => new CaepfModel(caepfBase: input).Unformatted(),
                    (Algorithm.cnpj, false) => new CnpjModel(input).Unformatted(),
                    (Algorithm.pis, false) => new PisPasepModel(input).Unformatted(),
                    (Algorithm.cpf, true) => new CpfModel(input).Formatted(),
                    (Algorithm.caepf, true) => new CaepfModel(caepfBase: input).Formatted(),
                    (Algorithm.cnpj, true) => new CnpjModel(input).Formatted(),
                    (Algorithm.pis, true) => new PisPasepModel(input).Formatted(),
                    (Algorithm.rg, _) => new RgSspSpModel(input).Unformatted(),
                    _ => throw new NotImplementedException(),
                });
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
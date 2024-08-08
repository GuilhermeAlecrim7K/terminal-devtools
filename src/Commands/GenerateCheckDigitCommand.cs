using System.CommandLine;
using System.CommandLine.Invocation;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand : Command
{
    private sealed class GenerateCheckDigitCommand : Command
    {
        private static readonly string[] Algorithms = ["cpf", "caepf", "pis", "cnpj", "rg"];
        private readonly Option<string> _algorithmOption = new Option<string>(["--algorithm", "-a"], "Specifies the target for the check digit algorithm.") { Arity = ArgumentArity.ExactlyOne, IsRequired = true }.FromAmong(Algorithms);
        public GenerateCheckDigitCommand() : base("check-digit", "Generates check digits.")
        {
            AddOption(_algorithmOption);
            AddArgument(new Argument<string>("input") { Arity = ArgumentArity.ExactlyOne });
            this.SetHandler(CommandHandler);
        }

        private void CommandHandler(InvocationContext context)
        {
            // TODO: Implementation
        }
    }
}
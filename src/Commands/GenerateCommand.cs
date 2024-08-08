using System.CommandLine;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand : Command
{
    private readonly GeneratePersonCommand _personCommand = new();
    private readonly GenerateCompanyCommand _companyCommand = new();
    private readonly GenerateCheckDigitCommand _checkDigitCommand = new();
    public GenerateCommand() : base(name: "generate", description: "A command for generating data")
    {
        AddCommand(_companyCommand);
        AddCommand(_personCommand);
        AddCommand(_checkDigitCommand);
    }

}
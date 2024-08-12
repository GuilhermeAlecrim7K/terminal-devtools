using System.CommandLine;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand : Command
{
    private readonly GeneratePersonCommand _personCommand;
    private readonly GenerateCompanyCommand _companyCommand;
    private readonly GenerateCheckDigitCommand _checkDigitCommand;
    private readonly Option<bool> _formatOption = new(aliases: ["--format", "-f"], description: "Determines if the numbers generated will be formatted", getDefaultValue: () => false);
    public GenerateCommand() : base(name: "generate", description: "A command for generating data")
    {
        _checkDigitCommand = new(_formatOption);
        _personCommand = new(_formatOption);
        _companyCommand = new(_formatOption);
        AddCommand(_companyCommand);
        AddCommand(_personCommand);
        AddCommand(_checkDigitCommand);
        AddGlobalOption(_formatOption);
    }

}
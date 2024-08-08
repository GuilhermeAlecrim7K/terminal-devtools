using System.CommandLine;

namespace TerminalDevTools.Commands;

internal sealed partial class GenerateCommand : Command
{
    private readonly GeneratePersonCommand _personCommand = new();
    private readonly GenerateCompanyCommand _companyCommand = new();
    public GenerateCommand() : base(name: "generate", description: "A command for generating data")
    {
        AddCommand(_companyCommand);
        AddCommand(_personCommand);
    }

}
using System.CommandLine;

namespace TerminalDevTools.Commands;
public class TerminalDevToolsRootCommand : RootCommand
{
    private readonly GenerateCommand _generateCommand = new();
    public TerminalDevToolsRootCommand() : base(description: "TerminalDevTools")
    {
        AddCommand(_generateCommand);
    }
}
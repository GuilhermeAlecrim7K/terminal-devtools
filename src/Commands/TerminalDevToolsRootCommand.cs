using System.CommandLine;

namespace TerminalDevTools.Commands;
public class TerminalDevToolsRootCommand : RootCommand
{
    public TerminalDevToolsRootCommand() : base(description: "TerminalDevTools")
    {
        AddCommand(new GenerateCommand());
        AddCommand(new EncodeCommand());
        AddCommand(new DecodeCommand());
        AddCommand(new PasswordCommand());
    }
}
using System.CommandLine;

namespace TerminalDevTools.Commands;
internal sealed class TerminalDevToolsRootCommand : RootCommand
{
    public TerminalDevToolsRootCommand() : base(description: "TerminalDevTools")
    {
        AddCommand(new GenerateCommand());
        AddCommand(new EncodeCommand());
        AddCommand(new DecodeCommand());
        AddCommand(new PasswordCommand());
    }
}
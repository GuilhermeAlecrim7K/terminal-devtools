using System.CommandLine;

namespace TerminalDevTools.Commands;

internal sealed class PasswordCommand : Command
{
    public PasswordCommand() : base(name: "password", description: "A password tool")
    {
        // TODO: Implement each subcommand in their own classes
        AddCommand(new Command(name: "store", description: "store a password in the database"));
        AddCommand(new Command(name: "retrieve", description: "retrieves a password from the database"));
        AddCommand(new Command(name: "generate", description: "generates random password"));
    }
}
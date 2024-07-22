using System.CommandLine;

using TerminalDevTools.Commands;

namespace TerminalDevTools;
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new TerminalDevToolsRootCommand();
        return await rootCommand.InvokeAsync(args);
    }
}
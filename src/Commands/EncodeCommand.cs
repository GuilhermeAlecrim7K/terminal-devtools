using System.CommandLine;
using System.CommandLine.Invocation;

namespace TerminalDevTools.Commands;

public class EncodeCommand : Command
{
    private readonly Option<EncodingOption> _encodingOption = new(aliases: ["--encoding", "-e"])
    {
        Arity = ArgumentArity.ExactlyOne,
        IsRequired = true,
    };
    private readonly Option<string> _fromStringOption = new(aliases: ["--from-string", "-s"]);
    private readonly Option<FileInfo> _fromFileInfoOption = new(aliases: ["--from-file", "-f"]);
    public EncodeCommand() : base(name: "encode", description: "An encoding tool")
    {
        AddOption(_encodingOption);
        AddOption(_fromStringOption);
        AddOption(_fromFileInfoOption);
        this.SetHandler(CommandHandler);
    }

    private void CommandHandler(InvocationContext context)
    {
        EncodingOption encoding = context.ParseResult.GetValueForOption(_encodingOption);
        string? fromString = context.ParseResult.GetValueForOption(_fromStringOption);
        FileInfo? fromFile = context.ParseResult.GetValueForOption(_fromFileInfoOption);
        try
        {
            if (string.IsNullOrEmpty(fromString) && fromFile is null)
                throw new ArgumentException(message: $"Error: You must provide either --{_fromStringOption.Name} or --{_fromFileInfoOption.Name}.");
            if (!string.IsNullOrEmpty(fromString) && fromFile is not null)
                throw new ArgumentException(message: $"Error: Only one of --{_fromStringOption.Name} or --{_fromFileInfoOption.Name} can be provided.");
            switch (encoding)
            {
                case EncodingOption.Base64:
                    // TODO:
                    /*
                        Must check encoding on files and string (e.g. Windows cmd which is not UTF-8) before doing the encoding
                    */
                    break;
                default:
                    throw new NotImplementedException();
            }
            context.ExitCode = 0;
        }
        catch (ArgumentException ex)
        {
            context.ExitCode = 1;
            context.Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            context.ExitCode = 1;
            context.Console.WriteLine(ex.ToString());
        }
    }
}
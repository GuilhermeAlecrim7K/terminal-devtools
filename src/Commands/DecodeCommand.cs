using System.CommandLine.Invocation;

namespace TerminalDevTools.Commands;

public class DecodeCommand() : EncodingCommand(name: "decode", description: "A decoding tool")
{
    protected override void HandleBase64(InvocationContext context, FileInfo fileInfo)
    {
        throw new NotImplementedException();
    }
    protected override void HandleBase64(InvocationContext context, string origin)
    {
        throw new NotImplementedException();
    }
}
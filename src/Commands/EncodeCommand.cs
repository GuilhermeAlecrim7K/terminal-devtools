using System.CommandLine.Invocation;

namespace TerminalDevTools.Commands;

internal sealed class EncodeCommand() : EncodingCommand(name: "encode", description: "An encoding tool")
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
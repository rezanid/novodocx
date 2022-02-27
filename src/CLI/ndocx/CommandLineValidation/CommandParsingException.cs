using System.CommandLine.Parsing;

namespace Novo.Docx.Cli;

internal class CommandParsingException : Exception
{
    public CommandParsingException(
        string message, 
        ParseResult parseResult = null) : base(message)
    {
        ParseResult = parseResult;
        Data.Add("CLI_User_Displayed_Exception", true);
    }

    public ParseResult ParseResult;
}

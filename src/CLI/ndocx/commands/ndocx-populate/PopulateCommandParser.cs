using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Novo.Docx.Tools;
using LocalizableStrings = Novo.Docx.Tools.Populate.LocalizableStrings;

namespace Novo.Docx.Cli;

internal class PopulateCommandParser
{
    public static readonly string DocsLink = "https://link-to-relevant-github-page";

    public static readonly Argument<string[]> TemplateArgument = new Argument<string[]>(LocalizableStrings.TemplateArgumentName)
    {
        Description = CommonLocalizableStrings.TemplateArgumentDescription,
        Arity = ArgumentArity.ZeroOrMore
    }.DefaultToCurrentDirectory();

    public static readonly Option<string> ParamsOption = new Option<string>(new string[] { "-p", "--params" }, LocalizableStrings.ParamsOptionDescription)
    {
        ArgumentHelpName = LocalizableStrings.ParamsArgumentName
    };

    public static readonly Option<string> OutputOption = new Option<string>(new string[] { "-o", "--output" }, LocalizableStrings.OutputOptionDescription)
    {
        ArgumentHelpName = LocalizableStrings.OutputArgumentName
    };

    private static readonly Command Command = ConstructCommand();

    public static Command GetCommand() => Command;

    private static Command ConstructCommand()
    {
        var command = new DocumentedCommand("populate", DocsLink, LocalizableStrings.PopulateCommand);

        command.AddArgument(TemplateArgument);
        command.AddOption(ParamsOption);
        command.AddOption(OutputOption);

        command.SetHandler(PopulateCommand.Run);

        return command;
    }
}

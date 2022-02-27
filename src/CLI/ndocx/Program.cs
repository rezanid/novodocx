using Novo.Docx.Cli.Utils;
using Novo.Docx.Tools.Help;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace Novo.Docx.Cli;

public class Program
{
    public static int Main(string[] args) 
    {
        DebugHelper.HandleDebugSwitch(ref args);

        InitializeProcess();

        try
        {
            return ProcessArgs(args);
        }
        catch (HelpException e)
        {
            Reporter.Output.WriteLine(e.Message);
            return 0;
        }
        catch (Exception e) when (e.ShouldBeDisplayedAsError())
        {
            Reporter.Error.WriteLine(CommandContext.IsVerbose()
                ? e.ToString().Red().Bold()
                : e.Message.Red().Bold());

            var commandParsingException = e as CommandParsingException;
            if (commandParsingException != null && commandParsingException.ParseResult != null)
            {
                commandParsingException.ParseResult.ShowHelp();
            }

            return 1;
        }
        catch (Exception e) when (!e.ShouldBeDisplayedAsError())
        {
            Reporter.Error.WriteLine(e.ToString().Red().Bold());

            return 1;
        }
    }

    internal static int ProcessArgs(string[] args) 
    {
        var parseResult = Parser.Instance.Parse(args);

        if (parseResult.HasOption(Parser.VersionOption) && parseResult.IsTopLevelDotnetCommand())
        {
            CommandLineInfo.PrintVersion();
            return 0;
        }
        else if (parseResult.HasOption(Parser.InfoOption) && parseResult.IsTopLevelDotnetCommand())
        {
            CommandLineInfo.PrintInfo();
            return 0;
        }
        else if (parseResult.HasOption(new Option<bool>("-h")) && parseResult.IsTopLevelDotnetCommand())
        {
            HelpCommand.PrintHelp();
            return 0;
        }

        int exitCode;
        if (parseResult.CanBeInvoked())
        {
            exitCode = parseResult.Invoke();
        }
        else
        {
            // NOTE!
            // Command extension through external assemblies and global installation
            // is not supported yet, hence I keep the following lines commented for 
            // the time being.
            //
            //var resolvedCommand = CommandFactoryUsingResolver.Create(
            //    "ndocx-" + parseResult.GetValueForArgument(Parser.DotnetSubCommand),
            //    args.GetSubArguments(),
            //    FrameworkConstants.CommonFrameworks.NetStandardApp15);
            //var result = resolvedCommand.Execute();
            //exitCode = result.ExitCode;
            exitCode = -1;
        }
        return exitCode;
    }

    private static void InitializeProcess()
    {
        // by default, .NET Core doesn't have all code pages needed for Console apps.
        // see the .NET Core Notes in https://docs.microsoft.com/dotnet/api/system.diagnostics.process#-notes
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
}

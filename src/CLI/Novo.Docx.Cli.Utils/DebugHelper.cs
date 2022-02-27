using System.Diagnostics;
namespace Novo.Docx.Cli.Utils;
public static class DebugHelper
{
    [Conditional("DEBUG")]
    public static void HandleDebugSwitch(ref string[] args)
    {
        if (args.Length > 0 && string.Equals("--debug", args[0], StringComparison.OrdinalIgnoreCase))
        {
            args = args.Skip(1).ToArray();
            WaitForDebugger();
        }
    }

    public static void WaitForDebugger()
    {
        int processId = Environment.ProcessId;
        Console.WriteLine(LocalizableStrings.WaitingForDebuggerToAttach);
        Console.WriteLine(string.Format(LocalizableStrings.ProcessId, processId));
        Console.ReadLine();
    }
}
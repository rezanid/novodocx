using System.CommandLine;

namespace Novo.Docx.Cli;
public class DocumentedCommand : Command
{
    public string DocsLink { get; set; }

    public DocumentedCommand(string name, string docsLink, string description = null) : base(name, description)
    {
        DocsLink = docsLink;
    }
}

using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Novo.DocumentService;
using System.CommandLine.Parsing;

namespace Novo.Docx.Cli;

internal class PopulateCommand 
{
    //TODO: Finish implementing PopulateCommand and all its requirements
    public static int Run(ParseResult parseResult)
    {
        parseResult.HandleDebugSwitch();

        var templateFilePaths = parseResult.GetValueForArgument(PopulateCommandParser.TemplateArgument) ?? Array.Empty<string>();
        var paramsFilePath = parseResult.GetValueForOption(PopulateCommandParser.ParamsOption);
        var outputFilePath = parseResult.GetValueForOption(PopulateCommandParser.OutputOption);

        //TODO: Default values? validations? null-checks? :)

        var documentProcessor = new WordDocumentProcessor(new NullLogger<WordDocumentProcessor>());
        foreach (var templateFilePath in templateFilePaths.Where(t => !string.IsNullOrWhiteSpace(t)))
        {
            if (string.IsNullOrWhiteSpace(outputFilePath))
            {
                outputFilePath = templateFilePath;
            }
            else
            {
                File.Copy(templateFilePath, outputFilePath, true);
            }
            using var fileStream = new FileStream(outputFilePath, FileMode.Open, FileAccess.ReadWrite);
            var parameters = JObject.Parse(File.ReadAllText(paramsFilePath!));

            documentProcessor.PopulateDocumentTemplate(parameters, fileStream);
        }

        return 0;
    }
}

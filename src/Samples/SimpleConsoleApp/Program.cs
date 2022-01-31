using System.IO;
using Novo.DocumentService;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging.Abstractions;

Console.WriteLine("Welcome to Novo DocX!");

using var fileStream = new FileStream("SimpleTemplate.docx", FileMode.Open, FileAccess.Read);
var base64 = fileStream.ConvertToBase64String();
var input = new JObject
{
    ["file"] = base64,
    ["parameters"] = new JObject()
    {
        ["textInRun"] = "Text in Run",
        ["textInRunInParagraph"] = "Text in Run in Paragraph",
        ["textInRunInParagraphInCell"] = "Text in Run in Paragraph in Cell",
        ["textInRunAllowMulti"] = "Text in Run Allow Multi Line 1\r\nLine 2.",
        ["textInRunWithPlaceholderText"] = "Text in Run with Placeholder Text",
    }
};
var docProcessor = new WordDocumentProcessor(NullLogger<WordDocumentProcessor>.Instance);
var result = docProcessor.PopulateDocumentTemplate(input);
if (result != null && result.Success && result.Result.TryGetValue("file", out var outputbase64))
{
    WriteBase64ToFile((string)outputbase64!,  "SimpleDocument.docx");
    Console.WriteLine("Template successfully filled and stored as SimpleDocument.docx");
}
else
{
    Console.WriteLine("Template population failed.");
}

void WriteBase64ToFile(string base64, string filePath)
{
    var buffer = new byte[(base64.Length * 3 + 3) / 4 -
        (base64.Length > 0 && base64[^1] == '=' ?
            base64.Length > 1 && base64[^2] == '=' ?
                2 : 1 : 0)];
    if (!Convert.TryFromBase64String(base64, buffer, out int writtenBytes))
    {
        throw new InvalidOperationException(
            $"Document is not in base64 format. Make sure you are sending a valid base64 encoded docx file.\r\n" +
            $"bytesParsed: {writtenBytes}");
    }
    using var memory = new MemoryStream(buffer);
    using var output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    memory.WriteTo(output);
}
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using Novo.DocumentService;

namespace Novo.DocumentService.Tests;
[TestClass]
public class WordDocumentProcessorTest
{
    [TestMethod]
    public void PopulateSimpleDocumentWithSeveralParameters()
    {
        // Arrange
        using var fileStream = new FileStream(Path.Combine(@"Samples", "SimpleDocument.docx"), FileMode.Open, FileAccess.Read);
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

        // Act
        var result = docProcessor.PopulateDocumentTemplate(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Result);
        Assert.IsNotNull(result.Result["file"]);
#if WRITEOUTPUT
        WriteBase64ToFile(result.Result["file"]!.ToString(), "SimpleDocument.filled.docx");
#endif
    }

    [TestMethod]
    public void PopulateComplexDocumentWithSeveralParameters()
    {
        // Arrange
        using var fileStream = new FileStream(Path.Combine(@"Samples", "ComplexDocument.docx"), FileMode.Open, FileAccess.Read);
        var base64 = fileStream.ConvertToBase64String();
        var input = JObject.Parse(File.ReadAllText(Path.Combine(@"Samples", "InputParameters.json")));
        input["file"] = base64;
        var docProcessor = new WordDocumentProcessor(NullLogger<WordDocumentProcessor>.Instance);

        // Act
        var result = docProcessor.PopulateDocumentTemplate(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Result);
        Assert.IsNotNull(result.Result["file"]);
#if WRITEOUTPUT
        WriteBase64ToFile(result.Result["file"]!.ToString(), "ComplexDocument.filled.docx");
#endif
    }

    [TestMethod]
    public void PopulateRealworldUTF16LE()
    {
        //using var fileStream = new FileStream(@"Test1.docx", FileMode.Open, FileAccess.Read);
        //var base64 = fileStream.ConvertToBase64String();
        //input["file"] = base64;
        // Arrange
        var input = JObject.Parse(File.ReadAllText(Path.Combine(@"Samples", "InputPayload.json")));
        var docProcessor = new WordDocumentProcessor(NullLogger<WordDocumentProcessor>.Instance);

        // Act
        var result = docProcessor.PopulateDocumentTemplate(input);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Result);
        Assert.IsNotNull(result.Result["file"]);
#if WRITEOUTPUT
        WriteBase64ToFile(result.Result["file"]!.ToString(), Path.Combine(@"Samples", "RealworldDocument.filled.docx"));
#endif
    }

    private void WriteBase64ToFile(string base64, string filePath)
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
}

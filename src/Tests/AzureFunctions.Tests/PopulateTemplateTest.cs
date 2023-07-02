using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using Novo.DocumentService;

namespace Novo.AzureFunctions.Tests;
[TestClass]
public class PopulateTemplateTest
{
    [TestMethod]
    public async Task PopulateTemplateNotThrow()
    {
        var request = CreateHttpRequest(Path.Combine("Samples","InputPayload.json"));
        var documentProcessor = new WordDocumentProcessor(new NullLogger<WordDocumentProcessor>());
        var azFunction = new PopulateTemplate(new NullLogger<PopulateTemplate>(), documentProcessor);

        var response = (OkObjectResult)await azFunction.Run(request);
        
        Assert.AreEqual(200, response.StatusCode);
        var result = (JObject)response.Value;
        Assert.IsNotNull(result);
        Assert.IsTrue(result.ContainsKey("file"));
#if WRITEOUTPUT
        WriteBase64ToFile(result["file"]!.ToString(), "RealworldDocument.filled.docx");
#endif
    }

    private static HttpRequest CreateHttpRequest(string filePath)
    {
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.Method = "POST";
        request.Body = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return request;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using Novo.DocumentService;
using Microsoft.Azure.Functions.Worker.Http;
using AzureFunctions.Tests;

namespace Novo.AzureFunctions.Tests;
[TestClass]
public class PopulateTemplateTest
{
    private PopulateTemplate _sut;

    [TestInitialize]
    public void Initialize()
    {
        var documentProcessor = new WordDocumentProcessor(new NullLogger<WordDocumentProcessor>());
        _sut = new PopulateTemplate(new NullLoggerFactory(), documentProcessor);
    }

    [TestMethod]
    public async Task PopulateTemplateNotThrow()
    {
        var request = CreateHttpRequest(Path.Combine("Samples","InputPayload.json"));
        var response = await _sut.Run(request);
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        var result = JObject.Parse(await response.GetResponseBodyAsync());
        Assert.IsNotNull(result);
        Assert.IsTrue(result.ContainsKey("file"));
#if WRITEOUTPUT
        WriteBase64ToFile(result["file"]!.ToString(), "RealworldDocument.filled.docx");
#endif
    }

    private static MockHttpRequestData CreateHttpRequest(string filePath) => 
        new("POST", new Uri("http://function/Word", UriKind.Absolute), File.ReadAllText(filePath));

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
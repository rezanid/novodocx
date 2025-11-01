using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Novo.Xrm.DocumentApi;

namespace Novo.Xrm.DocumentApi.Tests
{
    [TestClass]
    public class DocxTemplateProcessorTests
    {
        [TestMethod]
        public void Populate_Simple_PlainText_Controls()
        {
            using (var fs = new FileStream(Path.Combine("Samples", "SimpleDocument.docx"), FileMode.Open, FileAccess.Read))
            using (var ms = new MemoryStream())
            {
                fs.CopyTo(ms);
                var docBytes = ms.ToArray();
                var parameters = new JObject
                {
                    ["textInRun"] = "Text in Run",
                    ["textInRunInParagraph"] = "Text in Run in Paragraph",
                    ["textInRunInParagraphInCell"] = "Text in Run in Paragraph in Cell",
                    ["textInRunAllowMulti"] = "Text in Run Allow Multi Line1\r\nLine2.",
                    ["textInRunWithPlaceholderText"] = "Text in Run with Placeholder Text"
                };

                var sut = new DocxTemplateProcessor();
                var resultBytes = sut.Populate(docBytes, parameters);
                Assert.IsNotNull(resultBytes);
                Assert.IsTrue(resultBytes.Length > 0);
#if WRITEOUTPUT
                File.WriteAllBytes("SimpleDocument.filled.xrm.docx", resultBytes);
#endif
            }
        }

        [TestMethod]
        public void Populate_Complex_Repeating_And_Text()
        {
            using (var fs = new FileStream(Path.Combine("Samples", "ComplexDocument.docx"), FileMode.Open, FileAccess.Read))
            using (var ms = new MemoryStream())
            {
                fs.CopyTo(ms);
                var docBytes = ms.ToArray();
                var input = JObject.Parse(File.ReadAllText(Path.Combine("Samples", "InputParameters.json")));
                var sut = new DocxTemplateProcessor();
                var resultBytes = sut.Populate(docBytes, (JObject)input);
                Assert.IsNotNull(resultBytes);
                Assert.IsTrue(resultBytes.Length > 0);
#if WRITEOUTPUT
                File.WriteAllBytes("ComplexDocument.filled.xrm.docx", resultBytes);
#endif
            }
        }

        [TestMethod]
        public void Populate_RealWorld_Payload()
        {
            var input = JObject.Parse(File.ReadAllText(Path.Combine("Samples", "InputPayload.json")));
            // Input payload contains base64; mirror WordDocumentProcessor flow
            var base64 = (string)input["file"];
            var buffer = Convert.FromBase64String(base64);
            var parameters = (JObject)input["parameters"]!;
            var sut = new DocxTemplateProcessor();
            var resultBytes = sut.Populate(buffer, parameters);
            Assert.IsNotNull(resultBytes);
            Assert.IsTrue(resultBytes.Length > 0);
#if WRITEOUTPUT
            File.WriteAllBytes(Path.Combine("Samples", "RealworldDocument.xrm.filled.docx"), resultBytes);
#endif
        }
    }
}

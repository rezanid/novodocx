using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Novo.Xrm.DocumentApi
{
    /// <summary>
    /// Minimal, allocation-conscious DOCX placeholder processor that works with in-memory bytes only.
    /// Supports:
    /// - Plain text content controls (w:sdtPr/w:text) mapped by w:tag/@w:val
    /// - Repeating section content controls (w15:repeatingSection) with items (w15:repeatingSectionItem)
    /// Processes main document, headers, and footers.
    /// </summary>
    public class DocxTemplateProcessor
    {
        private static readonly XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        private static readonly XNamespace w15 = "http://schemas.microsoft.com/office/word/2012/wordml";
        private static readonly XNamespace xml = "http://www.w3.org/XML/1998/namespace";

        //10 MB default limit
        private const int MaxDocSizeBytes = 10 * 1024 * 1024;

        /// <summary>
        /// Populate placeholders in the given DOCX bytes using the provided parameters.
        /// </summary>
        /// <param name="docxBytes">Input .docx contents (ZIP) as bytes</param>
        /// <param name="parameters">JSON object with values for content controls; values can be primitives or arrays (for repeating sections)</param>
        /// <returns>New .docx bytes</returns>
        public byte[] Populate(byte[] docxBytes, JObject parameters)
        {
            if (docxBytes == null) throw new ArgumentNullException(nameof(docxBytes));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (docxBytes.Length > MaxDocSizeBytes)
                throw new InvalidOperationException("Document exceeds the10MB limit.");

            using (var input = new MemoryStream(docxBytes, writable: false))
            using (var sourceZip = new ZipArchive(input, ZipArchiveMode.Read, leaveOpen: true))
            using (var output = new MemoryStream())
            using (var targetZip = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true))
            {
                // Decide which XML parts to process
                var partNamesToProcess = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                partNamesToProcess.Add("word/document.xml");
                foreach (var e in sourceZip.Entries)
                {
                    if (e.FullName.StartsWith("word/header", StringComparison.OrdinalIgnoreCase) && e.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        partNamesToProcess.Add(e.FullName);
                    }
                    else if (e.FullName.StartsWith("word/footer", StringComparison.OrdinalIgnoreCase) && e.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        partNamesToProcess.Add(e.FullName);
                    }
                }

                foreach (var entry in sourceZip.Entries)
                {
                    if (!partNamesToProcess.Contains(entry.FullName))
                    {
                        CopyEntry(entry, targetZip);
                        continue;
                    }

                    // Load, process, and write back XML parts
                    XDocument xdoc;
                    using (var es = entry.Open())
                    using (var reader = new StreamReader(es, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
                    {
                        var xmlText = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(xmlText))
                        {
                            // Create empty entry to preserve structure
                            var emptyTarget = targetZip.CreateEntry(entry.FullName, CompressionLevel.Optimal);
                            continue;
                        }
                        xdoc = XDocument.Parse(xmlText, LoadOptions.PreserveWhitespace);
                    }

                    ProcessPart(xdoc, parameters);

                    var targetEntry = targetZip.CreateEntry(entry.FullName, CompressionLevel.Optimal);
                    using (var ws = targetEntry.Open())
                    using (var writer = new StreamWriter(ws, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)))
                    {
                        xdoc.Save(writer, SaveOptions.DisableFormatting);
                    }
                }

                targetZip.Dispose(); // flush
                return output.ToArray();
            }
        }

        private static void CopyEntry(ZipArchiveEntry source, ZipArchive targetZip)
        {
            var newEntry = targetZip.CreateEntry(source.FullName, CompressionLevel.Optimal);
            using (var src = source.Open())
            using (var dst = newEntry.Open())
            {
                src.CopyTo(dst);
            }
        }

        private static void ProcessPart(XDocument xdoc, JObject parameters)
        {
            if (xdoc?.Root == null) return;

            // Find top-level w:sdt (no ancestor w:sdt)
            var topLevelSdts = xdoc
            .Descendants(w + "sdt")
            .Where(e => !e.Ancestors(w + "sdt").Any())
            .ToList();

            foreach (var sdt in topLevelSdts)
            {
                PopulateSdt(parameters, sdt);
            }
        }

        private static void PopulateSdt(JObject parameters, XElement sdt)
        {
            if (sdt == null) return;
            var sdtPr = sdt.Element(w + "sdtPr");
            if (sdtPr == null) return;

            var tag = sdtPr.Element(w + "tag");
            var tagValAttr = tag != null ? tag.Attribute(w + "val") : null;
            var tagVal = tagValAttr != null ? tagValAttr.Value : null;
            if (string.IsNullOrWhiteSpace(tagVal))
            {
                return; // unsupported: placeholders without tags
            }

            JToken token;
            if (!parameters.TryGetValue(tagVal, out token))
            {
                return; // no mapping
            }

            // Plain text content control? (w:sdtPr/w:text)
            bool isPlainText = sdtPr.Element(w + "text") != null;
            // Repeating section? (w:sdtPr/w15:repeatingSection)
            bool isRepeating = sdtPr.Element(w15 + "repeatingSection") != null;

            if (isPlainText)
            {
                ReplacePlainTextContent(sdt, token);
            }
            else if (isRepeating)
            {
                ReplaceRepeatingSection(sdt, token);
            }
            else
            {
                // Unsupported placeholder type; ignore
            }
        }

        private static void ReplacePlainTextContent(XElement sdt, JToken token)
        {
            var sdtContent = sdt.Element(w + "sdtContent");
            if (sdtContent == null) return;

            // Remove showing placeholder flag if present
            var sdtPr = sdt.Element(w + "sdtPr");
            var showingPlcHdr = sdtPr != null ? sdtPr.Element(w + "showingPlcHdr") : null;
            showingPlcHdr?.Remove();

            // Remove any PlaceholderText run styles
            foreach (var rStyle in sdtContent.Descendants(w + "rStyle").Where(e => (string)e.Attribute(w + "val") == "PlaceholderText").ToList())
            {
                rStyle.Remove();
            }

            // Use first paragraph/run if available, otherwise create a minimal structure
            var firstPara = sdtContent.Descendants(w + "p").FirstOrDefault();
            var firstRun = sdtContent.Descendants(w + "r").FirstOrDefault();

            if (firstRun == null)
            {
                if (firstPara == null)
                {
                    // Create a paragraph and run
                    firstPara = new XElement(w + "p");
                    sdtContent.RemoveNodes();
                    sdtContent.Add(firstPara);
                }
                firstRun = new XElement(w + "r");
                firstPara.Add(firstRun);
            }

            // Clear all text nodes within the content control area
            foreach (var t in sdtContent.Descendants(w + "t").ToList())
            {
                t.Remove();
            }

            // Insert text nodes into the first run, with line breaks
            InsertTextNodes(firstRun, TokenToText(token));
        }

        private static void ReplaceRepeatingSection(XElement sdt, JToken token)
        {
            if (token == null || token.Type != JTokenType.Array) return;
            var array = (JArray)token;

            var sdtContent = sdt.Element(w + "sdtContent");
            if (sdtContent == null) return;

            var firstChild = sdtContent.Elements().FirstOrDefault();
            if (firstChild == null || firstChild.Name != w + "sdt")
            {
                // Unexpected structure; skip
                return;
            }

            var repeatingItemTemplate = new XElement(firstChild);

            // Remove existing children
            sdtContent.RemoveNodes();

            // Remove w:id from the template and its descendants
            foreach (var id in repeatingItemTemplate.DescendantsAndSelf().Where(e => e.Name == w + "id").ToList())
            {
                id.Remove();
            }

            foreach (var item in array.OfType<JObject>())
            {
                var clone = new XElement(repeatingItemTemplate);

                // Find sdt children that are top-level within this repeating item (their only sdt ancestor is the item itself)
                var childSdts = clone
                .Descendants(w + "sdt")
                .Where(e => !e.Ancestors(w + "sdt").Except(new[] { clone }).Any())
                .ToList();

                foreach (var child in childSdts)
                {
                    PopulateSdt(item, child);
                }

                sdtContent.Add(clone);
            }
        }

        private static string TokenToText(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return string.Empty;

            // For primitives, use ToString with invariant culture-like behavior
            switch (token.Type)
            {
                case JTokenType.String:
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.Boolean:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                case JTokenType.Date:
                    return token.ToString();
                default:
                    // For objects/arrays, serialize compactly
                    return token.ToString(Newtonsoft.Json.Formatting.None);
            }
        }

        private static void InsertTextNodes(XElement run, string textualData)
        {
            if (run == null) return;
            if (textualData == null) textualData = string.Empty;

            var newLineArray = new[] { Environment.NewLine, "\r\n", "\n", "\n\r" };
            var parts = textualData.Split(newLineArray, StringSplitOptions.None);

            bool first = true;
            foreach (var line in parts)
            {
                if (!first)
                {
                    run.Add(new XElement(w + "br"));
                }
                first = false;

                var t = new XElement(w + "t", new XAttribute(xml + "space", "preserve"))
                {
                    Value = line ?? string.Empty
                };
                run.Add(t);
            }
        }
    }
}

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Novo.DocumentService;

namespace Novo.AzureFunctions.OpenApi;
public class DocumentProcessingResultExample : OpenApiExample<DocumentProcessingResult>
{
    public override IOpenApiExample<DocumentProcessingResult> Build(NamingStrategy namingStrategy = null)
    {
        var example1 = new DocumentProcessingResult(
            true,
            new JObject(new JProperty(
                "file",
                "Base64 encoded bynary of a docx file pupolated with the given parameters.")));
        Examples.Add(OpenApiExampleResolver.Resolve("sample1", example1, namingStrategy));
        return this;
    }
}

public class DocumentProcessingResultBadRequestExample : OpenApiExample<DocumentProcessingResult>
{
    public override IOpenApiExample<DocumentProcessingResult> Build(NamingStrategy namingStrategy = null)
    {
        var example2 = new DocumentProcessingResult(
            false,
                           result: new ParsingError(
                    message: "Document is not in base64 format. Make sure you are sending a valid base64 encoded docx file.",
                    bytesParsed: 76));

        Examples.Add(OpenApiExampleResolver.Resolve("sample2", example2, namingStrategy));
        return this;
    }
}
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Novo.DocumentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Novo.AzureFunctions.OpenApi;

namespace Novo.AzureFunctions;

public class PopulateTemplate
{
    private readonly ILogger<PopulateTemplate> _logger;
    private readonly IDocumentProcessor _documentProcessor;

    public PopulateTemplate(ILogger<PopulateTemplate> log, IDocumentProcessor documentProcessor)
    {
        _logger = log;
        _documentProcessor = documentProcessor;
    }

    [FunctionName("Word")]
    [OpenApiOperation(operationId: nameof(PopulateTemplate))]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", typeof(WordRequestModel), Example = typeof(WordRequestModelExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DocumentProcessingResult), Example = typeof(DocumentProcessingResultExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(DocumentProcessingResult), Example = typeof(DocumentProcessingResultBadRequestExample))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        var encoding = DetectEncodingFromContentType(req);
        string body;
        try
        {
            body = await req.ReadAsStringAsync(encoding ?? Encoding.UTF8);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Unable to populate the document due to an error. {ex}");
            throw;
        }
        var input = JObject.Parse(body);

        var result = _documentProcessor.PopulateDocumentTemplate(input);

        return result.Success ? new OkObjectResult(result.Result) : new BadRequestObjectResult(result.Result);
    }
    
    Encoding DetectEncodingFromContentType(HttpRequest request)
    {
        if (request == null) { throw new ArgumentNullException(nameof(request)); }
        if (string.IsNullOrWhiteSpace(request.ContentType)) { return null; }
        var charsetPos = request.ContentType.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
        if (charsetPos == -1) { return null; }
        var charset = request.ContentType[(charsetPos + 7)..]?.Trim(" =;".ToCharArray());
        try
        {
            return Encoding.GetEncoding(charset);
        }
        catch (Exception)
        {
            _logger.LogWarning($"Unsupported encoding '{charset}' given in the charset of the request." +
                $" utf-8 will be used as fallback");
            return null;
        }
    }
}

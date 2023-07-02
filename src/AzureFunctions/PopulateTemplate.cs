using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Novo.DocumentService;

namespace Novo.AzureFunctions
{
    public class PopulateTemplate
    {
        private readonly ILogger<PopulateTemplate> _logger;
        private readonly IDocumentProcessor _documentProcessor;

        public PopulateTemplate(ILoggerFactory loggerFactory, IDocumentProcessor documentProcessor)
        {
            _logger = loggerFactory.CreateLogger<PopulateTemplate>();
            _documentProcessor = documentProcessor;
        }

        [Function("Word")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
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

            HttpResponseData resp;
            if (result.Success)
            {
                resp = req.CreateResponse(HttpStatusCode.OK);
                resp.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await resp.WriteStringAsync(result.Result.ToString());
                return resp;
            }
            resp = req.CreateResponse(HttpStatusCode.BadRequest);
            await resp.WriteStringAsync(result.Result.ToString());
            return resp;
        }

        Encoding? DetectEncodingFromContentType(HttpRequestData request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (!request.Headers.TryGetValues("Content-Type", out var values)) { return null; }
            var contentType = values.FirstOrDefault();
            if (contentType == null) { return null; }
            var charsetPos = contentType?.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
            if (!charsetPos.HasValue || charsetPos == -1) { return null; }
            var charset = contentType![(charsetPos.Value + 7)..]?.Trim(" =;".ToCharArray());
            if (charset == null) { return null; }
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
}

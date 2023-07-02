using Microsoft.Azure.Functions.Worker.Http;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions.Tests;
public static class HttpResponseDataExtensions
{
    public static async Task<string> GetResponseBodyAsync(this HttpResponseData response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(response.Body);
        return await reader.ReadToEndAsync();
    }
}

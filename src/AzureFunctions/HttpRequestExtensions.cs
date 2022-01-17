using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Novo;

internal static class HttpRequestExtensions
{
    public static async Task<string> ReadAsStringAsync(this HttpRequest request, Encoding encoding)
    {
        request.EnableBuffering();
        string result = null;
        using (var reader = new StreamReader(request.Body, encoding, detectEncodingFromByteOrderMarks: true, 1024, leaveOpen: true))
        {
            result = await reader.ReadToEndAsync();
        }
        request.Body.Seek(0L, SeekOrigin.Begin);
        return result;
    }
}

using Newtonsoft.Json.Linq;

namespace Novo.DocumentService;
public static class JsonExtensions
{
    public static bool TryGetValue<T>(this JObject obj, string propertyName, out T? value) where T : JToken
    {
        var succeeded = obj.TryGetValue(propertyName, out var v);
        value = succeeded ? v as T : default;
        return succeeded && value != null;
    }
}

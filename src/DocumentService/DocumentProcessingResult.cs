using Newtonsoft.Json.Linq;

namespace Novo.DocumentService;
public class DocumentProcessingResult(bool success, JObject result)
{
    public bool Success { get; init; } = success; public JObject Result { get; init; } = result;
}

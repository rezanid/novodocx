using Newtonsoft.Json.Linq;

namespace Novo.DocumentService;
public class Error
{
    public string Message { get; set; }
    public Error(string message) => Message = message;
    public static implicit operator JObject(Error value) => JObject.FromObject(value);
}

public class ParsingError : Error
{
    public ParsingError(string message, int bytesParsed) : base(message)
    {
        BytesParsed = bytesParsed;
    }

    public int BytesParsed { get; init; }
}

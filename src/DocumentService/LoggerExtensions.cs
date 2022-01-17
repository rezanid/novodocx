using Microsoft.Extensions.Logging;

namespace Novo.DocumentService;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception?> _processingTag;
    static LoggerExtensions()
    {
        _processingTag = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: 1,
            formatString: "Processing tag: '{Tag}'"
            );
    }
    public static void WordDocumentProcessorProcessingTag(this ILogger logger, string tag) => _processingTag(logger, tag, null);
}

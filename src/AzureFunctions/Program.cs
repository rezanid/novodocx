using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Novo.DocumentService;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    s.AddSingleton<IDocumentProcessor, WordDocumentProcessor>())
    .Build();

host.Run();

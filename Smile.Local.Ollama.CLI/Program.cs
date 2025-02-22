using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smile.Local.Ollama.Business.Services;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Data;

namespace Smile.Local.Ollama.CLI
{
    public class Program
    {
        public static string[] Args { get; private set; }

        static void Main(string[] args)
        {
            Args = args;

            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<IDBContext, DBContext>();
                services.AddTransient<IOllamaService, OllamaService>();

                services.AddTransient<IPdfDocumentService, PdfDocumentService>();
                services.AddTransient<IWordDocumentService, WordDocumentService>();

                services.AddHostedService<Worker>();
            }).Start();
        }
    }
}

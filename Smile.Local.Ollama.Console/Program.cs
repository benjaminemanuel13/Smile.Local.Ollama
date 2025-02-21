using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smile.Local.Ollama.Business.Services;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Console;
using Smile.Local.Ollama.Data;

namespace Smile.Local.Ollama
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<IDBContext, DBContext>();
                services.AddTransient<IOllamaService, OllamaService>();
                services.AddHostedService<Worker>();
            }).Start();
        }

        private async static void Go()
        {
            //OllamaService ollama = new OllamaService();

            //var res = await ollama.GetEmbeddings("Who is Ben Emanuel?");
        }
    }
}

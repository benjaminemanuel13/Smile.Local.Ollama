using Microsoft.Extensions.Hosting;
using Smile.Local.Ollama.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.CLI
{
    public class Worker : BackgroundService
    {
        private readonly IOllamaService _ollama;

        public Worker(IOllamaService ollama)
        {
            _ollama = ollama;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var args = Program.Args;
            args.Select(x => x.ToLower()).ToList();

            switch (args[0])
            {
                case "upload":
                    var stream = File.OpenRead(args[1]);
                    _ollama.UploadDocument(args[1], stream).Wait();
                    break;
                case "ask-documents":
                    _ollama.AskDocuments(args[1], Console.Write).Wait();
                    break;
                case "ask":
                    _ollama.Ask(args[1], Console.Write).Wait();
                    break;
                case "analyse":
                    _ollama.AnalyseImage(args[1], args[2], Console.Write).Wait();
                    break;
            }

            return Task.CompletedTask;
        }
    }
}

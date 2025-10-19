using Microsoft.Extensions.Hosting;
using Smile.Local.Ollama.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Console
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
            System.Console.WriteLine("Enter a message to send to the chatbot or type 'quit' to exit:");
            var entry = System.Console.ReadLine();

            while (entry != "quit")
            {
                _ollama.Ask(entry, WriteOut).Wait();
                
                // , "hf.co/microsoft/bitnet-b1.58-2B-4T-gguf"

                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine("Enter a message to send to the chatbot or type 'quit' to exit:");
                entry = System.Console.ReadLine();
            }

            return Task.CompletedTask;
        }

        private void WriteOut(string message) { 
            System.Console.Write(message);
        }
    }
}

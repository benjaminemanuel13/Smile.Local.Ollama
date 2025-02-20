using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Common.Ollama;
using Smile.Local.Ollama.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Smile.Local.Ollama.Business.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly IDBContext _db;

        public OllamaService(DBContext db)
        {
            _db = db;
        }

        private string baseUrl = "http://localhost:11434/";

        public async Task<float[][]> GetEmbeddings(string text, string model = "all-minilm")
        {
            var url = baseUrl + "api/embed";

            var req = new EmbeddingRequest() {
                model = model,
                input = text
            };

            var json = JsonConvert.SerializeObject(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var res = await client.PostAsync(url, content);

            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                var embeddings = JsonConvert.DeserializeObject<EmbeddingResponse>(data);
                return embeddings.embeddings;
            }
            else
            {
                return null;
            }
        }

        public async Task AskDocuments(string prompt, Action<string> sendTo)
        {
            var docs = _db.GetDocuments(prompt);

            if (docs.Count() == 0)
            {
                sendTo("Sorry, No Answer Found.");

                return;
            }

            sendTo("\r\n\r\n");

            StringBuilder sb = new StringBuilder();

            sb.Append("<|system|>You are an assistant who answers questions from documents<|end|><|user|> From these documents (each document starts with <DOCUMENT> and ends with <ENDDOCUMENT>) answer this question:\"" + prompt + "\"");

            foreach (var doc in docs)
            {
                sb.AppendLine("<DOCUMENT>");
                sb.AppendLine(doc);
                sb.AppendLine("<ENDDOCUMENT>");
            }

            prompt = sb.ToString();
            prompt += "<|end|><|assistant|>";

            // Get Answer From Ollama into text and:
            // sendTo(text);

            //Run(prompt, sendTo);

            sendTo("\r\n\r\n");
        }
    }
}

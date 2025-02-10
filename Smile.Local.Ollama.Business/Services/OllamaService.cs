using Newtonsoft.Json;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Common.Ollama;
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
    }
}

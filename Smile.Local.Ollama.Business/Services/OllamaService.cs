using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Validation;
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
using static System.Net.WebRequestMethods;

namespace Smile.Local.Ollama.Business.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly IDBContext _db;
        private readonly IPdfDocumentService _pdf;
        private readonly IWordDocumentService _word;

        public OllamaService(IDBContext db, IPdfDocumentService pdf, IWordDocumentService word)
        {
            _db = db;
            _pdf = pdf;
            _word = word;
        }

        private string baseUrl = "http://localhost:11434/";

        public async Task<EmbeddingsModel> GetEmbeddings(string text, string model = "mxbai-embed-large")
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

                var mode = new EmbeddingsModel()
                {
                    data = new List<EmbeddingModel>() {
                        new EmbeddingModel(){
                            embedding = embeddings.embeddings[0]

                        }
                    }
                };

                return mode;
            }
            else
            {
                return null;
            }
        }

        private float[] Normalize(float[] embedding)
        {
            var norm = Math.Sqrt(embedding.Sum(x => x * x));
            return embedding.Select(x => (float)(x / norm)).ToArray();
        }

        public async Task AskDocuments(string prompt, Action<string> sendTo, string model = "phi4")
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

            await GetStreamAndSend(prompt, sendTo, model);

            sendTo("\r\n\r\n");
        }

        public async Task Ask(string prompt, Action<string> sendTo, string model = "phi4")
        {            
            prompt = "<|system|>You are an assistant who answers questions<|end|><|user|>" + prompt + "<|end|><|assistant|>";

            await GetStreamAndSend(prompt, sendTo, model);
        }

        private async Task GetStreamAndSend(string prompt, Action<string> sendTo, string model)
        {
            HttpClient client = new HttpClient();

            ChatRequest chatRequest = new ChatRequest()
            {
                model = model,
                messages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        role = "user",
                        content = prompt
                    }
                },
                stream = true
            };

            string json = JsonConvert.SerializeObject(chatRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, baseUrl + "api/chat") { 
                    Content = content
                };

                HttpResponseMessage response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = await reader.ReadLineAsync();

                                var resp = JsonConvert.DeserializeObject<ChatResponse>(line);

                                sendTo(resp.message.content);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sendTo("Sorry, I am not able to answer this question.");
            }
        }

        public async Task UploadDocument(string filename, Stream file)
        {
            filename = Path.GetFileName(filename).ToLower();
            var path = Directory.GetCurrentDirectory() + "\\Temp\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fullname = path + filename;

            using (var fileStream = new FileStream(fullname, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string data = string.Empty;

            try
            {
                if (filename.ToLower().EndsWith(".docx"))
                {
                    data = _word.TextFromWord(fullname);
                }
                else if (filename.ToLower().EndsWith(".pdf"))
                {
                    data = _pdf.Extract(fullname);
                }
                else if (filename.ToLower().EndsWith(".txt") || filename.ToLower().EndsWith(".json") || filename.ToLower().EndsWith(".csv"))
                {
                    var stream = System.IO.File.Open(fullname, FileMode.Open);
                    StreamReader reader = new StreamReader(stream);

                    data = reader.ReadToEnd();

                    reader.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                
            }

            float[] embeddings = (await GetEmbeddings(data)).data[0].embedding;

            var id = _db.SaveDocument("title", data);

            _db.SaveDocumentEmbeddings(id, embeddings);

            System.IO.File.Delete(fullname);
        }

        public async Task AnalyseImage(string path, string prompt, Action<string> sendTo, string model = "llama3.2-vision")
        {
            using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
            {
                byte[] bytes = new BinaryReader(stream).ReadBytes((int)stream.Length);
                string encoded = Convert.ToBase64String(bytes);

                var url = baseUrl + "api/generate";

                var req = new ImageRequest()
                {
                    model = model,
                    prompt = prompt,
                    images = new string[] { encoded }
                };

                string json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                try
                {
                    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, baseUrl + "api/generate")
                    {
                        Content = content
                    };

                    HttpResponseMessage response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    string line = await reader.ReadLineAsync();

                                    var resp = JsonConvert.DeserializeObject<ImageResponse>(line);

                                    sendTo(resp.response);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { 
                }
            }
        }
    }
}

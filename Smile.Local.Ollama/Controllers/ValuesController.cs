using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Data;
using Smile.Local.Ollama.Models;
using static System.Net.WebRequestMethods;

namespace Smile.Local.Ollama.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IOllamaService _ollama;
        private readonly IHttpContextAccessor _http;
        private readonly IPdfDocumentService _pdf;
        private readonly IWordDocumentService _word;
        private readonly IDBContext _db;

        public ValuesController(IOllamaService ollama, IHttpContextAccessor http, IPdfDocumentService pdf, IWordDocumentService word, IDBContext db)
        {
            _ollama = ollama;
            _http = http;
            _pdf = pdf;
            _word = word;
            _db = db;
        }

        [HttpPost("/embeddings")]
        public async Task<float[][]> GetEmbeddings([FromBody] Embeddings text)
        {
            return await _ollama.GetEmbeddings(text.text);
        }

        [HttpPost("/upload")]
        public async void UploadDocument(string author)
        {
            var file = _http.HttpContext.Request.Form.Files[0];

            var filename = Path.GetFileName(file.FileName).ToLower();
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

            float[] embeddings = (await _ollama.GetEmbeddings(data))[0];

            var id = _db.SaveDocument("title", data);

            _db.SaveDocumentEmbeddings(id, embeddings);

            System.IO.File.Delete(fullname);
        }
    }
}

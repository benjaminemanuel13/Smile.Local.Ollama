using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Common;
using Smile.Local.Ollama.Common.Ollama;
using Smile.Local.Ollama.Models;
using static System.Net.WebRequestMethods;

namespace Smile.Local.Ollama.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IOllamaService _ollama;
        private readonly IHttpContextAccessor _http;
        /*private readonly IPdfDocumentService _pdf;
        private readonly IWordDocumentService _word;
        private readonly IDBContext _db;*/

        public ValuesController(IOllamaService ollama, IHttpContextAccessor http /*, IPdfDocumentService pdf, IWordDocumentService word, IDBContext db*/)
        {
            _ollama = ollama;
            _http = http;
            /*_pdf = pdf;
            _word = word;
            _db = db;*/
        }

        [HttpPost("/embeddings")]
        public async Task<EmbeddingsModel> GetEmbeddings([FromBody] Embeddings text)
        {
            return await _ollama.GetEmbeddings(text.text);
        }

        [HttpPost("/upload")]
        public async void UploadDocument(string author)
        {
            var file = _http.HttpContext.Request.Form.Files[0];

            await _ollama.UploadDocument(file.FileName, file.OpenReadStream());
        }

        public async Task<IActionResult> Ask([FromBody] AskModel model)
        {
            await _ollama.Ask(model.question, async (response) =>
            {
                await Response.WriteAsync(response);
                await Response.Body.FlushAsync();
            });
            return new EmptyResult();
        }

        public async Task<IActionResult> AskDocuments([FromBody] AskDocumentsModel model)
        {
            await _ollama.AskDocuments(model.question, async (response) =>
            {
                await Response.WriteAsync(response);
                await Response.Body.FlushAsync();
            });
            return new EmptyResult();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smile.Local.Ollama.Business.Services.Interfaces;
using Smile.Local.Ollama.Models;

namespace Smile.Local.Ollama.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IOllamaService _ollama;

        public ValuesController(IOllamaService ollama)
        {
            _ollama = ollama;
        }

        [HttpPost("/embeddings")]
        public async Task<float[][]> GetEmbeddings([FromBody] Embeddings text)
        {
            return await _ollama.GetEmbeddings(text.text);
        }
    }
}


using Smile.Local.Ollama.Common.Ollama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Business.Services.Interfaces
{
    public interface IOllamaService
    {
        Task<EmbeddingsModel> GetEmbeddings(string text, string model = "mxbai-embed-large");

        Task Ask(string prompt, Action<string> sendTo, string model = "phi4");

        Task AskDocuments(string prompt, Action<string> sendTo, string model = "phi4");

        Task UploadDocument(string filename, Stream file);
    }
}

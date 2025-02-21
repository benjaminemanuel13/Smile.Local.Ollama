using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Business.Services.Interfaces
{
    public interface IOllamaService
    {
        Task<float[][]> GetEmbeddings(string text, string model = "all-minilm");

        Task Ask(string prompt, Action<string> sendTo, string model = "phi4");

        Task AskDocuments(string prompt, Action<string> sendTo, string model = "phi4");
    }
}

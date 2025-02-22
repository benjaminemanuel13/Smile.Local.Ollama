using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class EmbeddingsModel
    {
        public string @object { get; set; } = "list";

        public List<EmbeddingModel> data { get; set; }
    }
}

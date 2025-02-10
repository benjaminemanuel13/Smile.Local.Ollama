using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class EmbeddingRequest
    {
        public string model { get; set; }

        public string input { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class EmbeddingModel
    {
        public string @object { get; set; } = "embedding";

        public int index { get; set; }

        public float[] embedding {  get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class ChatRequest
    {
        public string model { get; set; }

        public List<ChatMessage> messages { get; set; }

        public bool stream { get; set; }
    }
}

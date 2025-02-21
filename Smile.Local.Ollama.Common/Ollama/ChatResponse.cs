using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class ChatResponse
    {
        public ChatMessage message { get; set; }

        public bool done { get; set; }
    }
}

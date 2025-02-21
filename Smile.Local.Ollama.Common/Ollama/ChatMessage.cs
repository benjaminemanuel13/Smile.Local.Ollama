using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class ChatMessage
    {
        public string role { get; set; } = "user";

        public string content { get; set; }
    }
}

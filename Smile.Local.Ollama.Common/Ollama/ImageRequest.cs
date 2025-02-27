using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Common.Ollama
{
    public class ImageRequest
    {
        public string model { get; set; }

        public string prompt { get; set; }

        public string[] images { get; set; }    
    }
}

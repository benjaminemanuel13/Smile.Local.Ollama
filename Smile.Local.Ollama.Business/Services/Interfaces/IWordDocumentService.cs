using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Business.Services.Interfaces
{
    public interface IWordDocumentService
    {
        string TextFromWord(string file);
    }
}

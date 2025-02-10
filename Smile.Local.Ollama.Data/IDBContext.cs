using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Data
{
    public interface IDBContext
    {
        List<string> GetDocuments(string text);
        int SaveDocument(string title, string text);
        void SaveDocumentEmbeddings(int id, float[] embeddings);
    }
}

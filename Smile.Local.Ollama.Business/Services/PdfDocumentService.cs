using iTextSharp.text.pdf;
using Smile.Local.Ollama.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Smile.Local.Ollama.Business.Services
{
    public class PdfDocumentService : IPdfDocumentService
    {
        public string Extract(string path)
        {
            var sb = new StringBuilder();
            var reader = new PdfReader(path);

            for (int i = 1; i < reader.NumberOfPages + 1; i++)
            {
                var streamBytes = reader.GetPageContent(i);
                var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(streamBytes));

                while (tokenizer.NextToken())
                {
                    if (tokenizer.TokenType == PrTokeniser.TK_STRING)
                    {
                        var currentText = tokenizer.StringValue;
                        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        sb.Append(tokenizer.StringValue);
                    }
                }

                sb.AppendLine();
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

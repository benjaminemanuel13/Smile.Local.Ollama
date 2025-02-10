using Smile.Local.Ollama.Business.Services;

namespace Smile.Local.Ollama
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Go();
            Console.ReadLine();
        }

        private async static void Go()
        {
            OllamaService ollama = new OllamaService();

            var res = await ollama.GetEmbeddings("Who is Ben Emanuel?");
        }
    }
}

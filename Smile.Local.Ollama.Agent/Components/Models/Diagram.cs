namespace Smile.Local.Ollama.Agent.Components.Models
{
    public class Diagram
    {
        public List<DiagramShape> Shapes { get; set; } = new();
        public List<Connector> Connectors { get; set; } = new();
    }
}

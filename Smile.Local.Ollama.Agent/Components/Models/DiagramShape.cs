namespace Smile.Local.Ollama.Agent.Components.Models
{
    public enum ShapeType
    {
        Agent,
        Plugin,
        ChatInit,
        ScheduledInit
    }

    public class DiagramShape
    {
        public string Id { get; set; }
        public string Shape { get; set; }
        public ShapeType ShapeType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsSelected { get; set; } = false;

        public string Label { get; set; }
    }

    public class Connector
    {
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Label { get; set; }
    }

    
}

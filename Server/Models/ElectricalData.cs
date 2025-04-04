namespace BlazorAIChatBot.Server.Models
{
    public class ElectricalData
    {
        public DateOnly Date { get; set; }
        public string Entity { get; set; } = string.Empty;
        public double DemandTWh { get; set; }
        public double GenerationTWh { get; set; }
    }
}
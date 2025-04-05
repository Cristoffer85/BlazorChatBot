namespace BlazorAIChatBot.Shared.Models
{
    public class ElectricalData
    {
        public DateOnly Date { get; set; }
        public string Entity { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public double DemandTWh { get; set; }
        public double GenerationTWh { get; set; }
    }
}
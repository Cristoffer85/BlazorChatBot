using BlazorAIChatBot.Shared.Models;
using System.Text.RegularExpressions;

namespace BlazorAIChatBot.Shared.BotTraining
{
    public static class BotTraining
    {
        public static (string entity, string startDate, string endDate) ParseQuestion(string question)
        {
            // Example: "Show me data for Germany from 2023-01 to 2023-12"
            string entity = string.Empty;
            string startDate = string.Empty;
            string endDate = string.Empty;

            // Extract entity (e.g., country name)
            var entityMatch = Regex.Match(question, @"data for (\w+)");
            if (entityMatch.Success)
            {
                entity = entityMatch.Groups[1].Value;
            }

            // Extract start date
            var startDateMatch = Regex.Match(question, @"from (\d{4}-\d{2})");
            if (startDateMatch.Success)
            {
                startDate = startDateMatch.Groups[1].Value;
            }

            // Extract end date
            var endDateMatch = Regex.Match(question, @"to (\d{4}-\d{2})");
            if (endDateMatch.Success)
            {
                endDate = endDateMatch.Groups[1].Value;
            }

            return (entity, startDate, endDate);
        }

        public static string GenerateResponseFromData(List<ElectricalData> data)
        {
            // Example: Summarize the data into a response
            var totalGeneration = data.Sum(d => d.GenerationTWh);
            var totalDemand = data.Sum(d => d.DemandTWh);

            return $"For the specified period, the total electricity generation was {totalGeneration:F2} TWh, and the total demand was {totalDemand:F2} TWh.";
        }
    }
}
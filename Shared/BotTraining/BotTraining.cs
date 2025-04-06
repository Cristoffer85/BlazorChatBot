using BlazorAIChatBot.Shared.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorAIChatBot.Shared.BotTraining
{
    public static class BotTraining
    {
        public static (string entity, string startDate, string endDate) ParseQuestion(string question)
        {
            string entity = string.Empty;
            string startDate = string.Empty;
            string endDate = string.Empty;

            // Normalize the input (trim and remove extra spaces)
            question = Regex.Replace(question.Trim(), @"\s+", " ");

            // Extract entity (e.g., country name)
            var entityMatch = Regex.Match(question, @"^(show me data for\s+)?([a-zA-Z\s]+?)(?=\s+\d{4}| from| to| [a-zA-Z]+\s+\d{4})", RegexOptions.IgnoreCase);
            if (entityMatch.Success)
            {
                entity = entityMatch.Groups[2].Value.Trim();
            }

            // Extract start and end dates in "YYYY-MM" format
            var dateRangeMatch = Regex.Match(question, @"(\d{4}-\d{2})\s+to\s+(\d{4}-\d{2})", RegexOptions.IgnoreCase);
            if (dateRangeMatch.Success)
            {
                startDate = dateRangeMatch.Groups[1].Value;
                endDate = dateRangeMatch.Groups[2].Value;
            }
            else
            {
                // Extract start and end dates in "Month YYYY" format
                var dateRangeVerboseMatch = Regex.Match(question, @"([a-zA-Z]+\s+\d{4})\s+to\s+([a-zA-Z]+\s+\d{4})", RegexOptions.IgnoreCase);
                if (dateRangeVerboseMatch.Success)
                {
                    startDate = ParseVerboseDate(dateRangeVerboseMatch.Groups[1].Value);
                    endDate = ParseVerboseDate(dateRangeVerboseMatch.Groups[2].Value);
                }
            }

            return (entity, startDate, endDate);
        }

        private static string ParseVerboseDate(string date)
        {
            // Normalize the input to ensure consistent parsing
            date = date.Trim();

            // Convert "Month YYYY" to "YYYY-MM"
            if (DateTime.TryParseExact(date, "MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate.ToString("yyyy-MM");
            }

            // Attempt to parse abbreviated month names (e.g., "Apr 2023")
            if (DateTime.TryParseExact(date, "MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM");
            }

            return string.Empty; // Return empty if parsing fails
        }

        public static string GenerateResponseFromData(List<ElectricalData> data, string entity, string startDate, string endDate)
        {
            if (data == null || !data.Any())
            {
                return $"I couldn't find any data for {entity} from {startDate} to {endDate}. Please check your query or try a different time period.";
            }

            // Example: Summarize the data into a response
            var totalGeneration = data.Sum(d => d.GenerationTWh);
            var totalDemand = data.Sum(d => d.DemandTWh);

            return $"For {entity} from {startDate} to {endDate}, the total electricity generation was {totalGeneration:F2} TWh, and the total demand was {totalDemand:F2} TWh.";
        }
    }
}
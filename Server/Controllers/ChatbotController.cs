using Microsoft.AspNetCore.Mvc;
using BlazorAIChatBot.Server.Services;
using BlazorAIChatBot.Shared.Models;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly ElectricalDataService _electricalDataService;

    public ChatbotController(ElectricalDataService electricalDataService)
    {
        _electricalDataService = electricalDataService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return Ok(new ChatResponse
            {
                Sender = "AI",
                Text = "I didn't understand your query. Please ask something like 'Show me data for Germany from 2023-01 to 2023-12.'"
            });
        }

        // Parse the user's question to extract parameters
        var (entity, startDate, endDate) = ParseQuestion(request.Question);

        if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
        {
            return Ok(new ChatResponse
            {
                Sender = "AI",
                Text = "I couldn't extract valid parameters from your query. Please try again with a proper format."
            });
        }

        // Retrieve the API key
        var apiKey = Environment.GetEnvironmentVariable("EMBER_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            return StatusCode(500, new ChatResponse
            {
                Sender = "AI",
                Text = "The server is not configured properly. Please contact support."
            });
        }

        // Fetch data using ElectricalDataService
        var data = await _electricalDataService.FetchElectricalDataAsync(entity, startDate, endDate, apiKey);

        if (data == null || !data.Any())
        {
            return Ok(new ChatResponse
            {
                Sender = "AI",
                Text = "I couldn't find any relevant data for your query. Please try again with different parameters."
            });
        }

        // Generate a response based on the fetched data
        var responseText = GenerateResponseFromData(data);

        return Ok(new ChatResponse
        {
            Sender = "AI",
            Text = responseText
        });
    }

    private static (string entity, string startDate, string endDate) ParseQuestion(string question)
    {
        // Example: "Show me data for Germany from 2023-01 to 2023-12"
        string entity = string.Empty;
        string startDate = string.Empty;
        string endDate = string.Empty;

        // Extract entity (e.g., country name)
        var entityMatch = System.Text.RegularExpressions.Regex.Match(question, @"data for (\w+)");
        if (entityMatch.Success)
        {
            entity = entityMatch.Groups[1].Value;
        }

        // Extract start date
        var startDateMatch = System.Text.RegularExpressions.Regex.Match(question, @"from (\d{4}-\d{2})");
        if (startDateMatch.Success)
        {
            startDate = startDateMatch.Groups[1].Value;
        }

        // Extract end date
        var endDateMatch = System.Text.RegularExpressions.Regex.Match(question, @"to (\d{4}-\d{2})");
        if (endDateMatch.Success)
        {
            endDate = endDateMatch.Groups[1].Value;
        }

        return (entity, startDate, endDate);
    }

    private string GenerateResponseFromData(List<ElectricalData> data)
    {
        // Example: Summarize the data into a response
        var totalGeneration = data.Sum(d => d.GenerationTWh);
        var totalDemand = data.Sum(d => d.DemandTWh);

        return $"For the specified period, the total electricity generation was {totalGeneration:F2} TWh, and the total demand was {totalDemand:F2} TWh.";
    }

    public class ChatRequest
    {
        public string Question { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
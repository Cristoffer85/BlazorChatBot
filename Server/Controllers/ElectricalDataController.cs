using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ElectricalDataController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ElectricalDataController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("fetch")]
    public async Task<IActionResult> FetchElectricalData(string entity, string startDate, string endDate)
    {
        try
        {
            // Load the API key from environment variables (create a .env file in the root of your project 
            // using: dotnet add package DotNetEnv) then add .env file in the root of the project with the following content: EMBER_API_KEY=<your-apikey-here>)
            DotNetEnv.Env.Load();
            string? apiKey = Environment.GetEnvironmentVariable("EMBER_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("API key is missing.");
            }

            // Fetch demand data
            string demandApiUrl = $"https://api.ember-energy.org/v1/electricity-demand/monthly?entity={entity}&start_date={startDate}&end_date={endDate}&api_key={apiKey}";
            var demandResponse = await _httpClient.GetStringAsync(demandApiUrl);
            var demandData = JsonSerializer.Deserialize<ApiResponse>(demandResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Fetch generation data
            string generationApiUrl = $"https://api.ember-energy.org/v1/electricity-generation/monthly?entity={entity}&start_date={startDate}&series=total%20generation&is_aggregate_series=true&api_key={apiKey}";
            var generationResponse = await _httpClient.GetStringAsync(generationApiUrl);
            var generationData = JsonSerializer.Deserialize<ApiResponse>(generationResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Combine demand and generation data
            if (demandData?.Data != null && generationData?.Data != null)
            {
                var electricalData = demandData.Data.Select(d => new ElectricalData
                {
                    Date = DateOnly.Parse(d.Date),
                    Entity = d.Entity,
                    DemandTWh = d.DemandTWh,
                    GenerationTWh = generationData.Data.FirstOrDefault(g => g.Date == d.Date)?.GenerationTWh ?? 0
                }).ToList();

                return Ok(electricalData);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    public class ElectricalData
    {
        public DateOnly Date { get; set; }
        public string Entity { get; set; } = string.Empty;
        public double DemandTWh { get; set; }
        public double GenerationTWh { get; set; }
    }

    public class ApiResponse
    {
        public List<ApiData> Data { get; set; } = [];
    }

    public class ApiData
    {
        [System.Text.Json.Serialization.JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("entity")]
        public string Entity { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("demand_twh")]
        public double DemandTWh { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("generation_twh")]
        public double GenerationTWh { get; set; }
    }
}
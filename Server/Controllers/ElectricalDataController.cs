using BlazorAIChatBot.Server.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ElectricalDataController(ElectricalDataService electricalDataService) : ControllerBase
{
    private readonly ElectricalDataService _electricalDataService = electricalDataService;

    // Fetches data from ember-energy.org API and returns it to the client.
    // Sign up for key at https://ember-energy.org/data/api/
    [HttpGet("fetch")]
    public async Task<IActionResult> FetchElectricalData(string entity, string startDate, string endDate)
    {
        try
        {
            // Load the API key from environment variables (create a .env file in the root of your project 
            // using: dotnet add package DotNetEnv) then add .env file in the root of the project with the following content: EMBER_API_KEY=<your-apikey-here>
            DotNetEnv.Env.Load();
            string? apiKey = Environment.GetEnvironmentVariable("EMBER_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("API key is missing.");
            }

            // Define URLs
            string demandApiUrl = $"https://api.ember-energy.org/v1/electricity-demand/monthly?entity={entity}&start_date={startDate}&end_date={endDate}&api_key={apiKey}";
            string generationApiUrl = $"https://api.ember-energy.org/v1/electricity-generation/monthly?entity={entity}&start_date={startDate}&series=total%20generation&is_aggregate_series=true&api_key={apiKey}";

            // Fetch data using the service
            var electricalData = await _electricalDataService.FetchElectricalDataAsync(demandApiUrl, generationApiUrl);

            if (electricalData.Any())
            {
                return Ok(electricalData);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
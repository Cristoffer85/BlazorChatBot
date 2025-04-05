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
    public async Task<IActionResult> FetchElectricalData(string entity, string startDate, string endDate, string? series = null)
    {
        try
        {
            // Load the API key from environment variables
            DotNetEnv.Env.Load();
            string? apiKey = Environment.GetEnvironmentVariable("EMBER_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("API key is missing.");
            }

            // Fetch data using the service
            var electricalData = await _electricalDataService.FetchElectricalDataAsync(entity, startDate, endDate, apiKey, series);

            // Always return a JSON response (even if the list is empty)
            return Ok(electricalData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
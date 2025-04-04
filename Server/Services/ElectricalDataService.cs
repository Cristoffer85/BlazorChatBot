using BlazorAIChatBot.Server.Models;
using System.Text.Json;

namespace BlazorAIChatBot.Server.Services
{
    public class ElectricalDataService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<ElectricalData>> FetchElectricalDataAsync(string demandApiUrl, string generationApiUrl)
        {
            // Fetch demand data
            var demandResponse = await _httpClient.GetStringAsync(demandApiUrl);
            var demandData = JsonSerializer.Deserialize<ApiResponse>(demandResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Fetch generation data
            var generationResponse = await _httpClient.GetStringAsync(generationApiUrl);
            var generationData = JsonSerializer.Deserialize<ApiResponse>(generationResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Combine demand and generation data
            if (demandData?.Data != null && generationData?.Data != null)
            {
                return demandData.Data.Select(d => new ElectricalData
                {
                    Date = DateOnly.Parse(d.Date),
                    Entity = d.Entity,
                    DemandTWh = d.DemandTWh,
                    GenerationTWh = generationData.Data.FirstOrDefault(g => g.Date == d.Date)?.GenerationTWh ?? 0
                }).ToList();
            }

            return [];
        }
    }
}
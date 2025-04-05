using BlazorAIChatBot.Shared.Models;
using System.Text.Json;
using BlazorAIChatBot.Server.Helpers;

namespace BlazorAIChatBot.Server.Services
{
    public class ElectricalDataService
    {
        private readonly HttpClient _httpClient;

        public ElectricalDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ElectricalData>> FetchElectricalDataAsync(
            string entity,
            string startDate,
            string endDate,
            string apiKey,
            string? series = null)
        {
            // Get the entity code for the given entity
            var entityCode = EntityCodeMapper.GetEntityCode(entity);
            if (string.IsNullOrEmpty(entityCode))
            {
                Console.WriteLine($"Error: Unknown entity '{entity}'.");
                return new List<ElectricalData>(); // Return an empty list if the entity is not recognized
            }

            // 1. Build the generation API URL
            string generationApiUrl = $"https://api.ember-energy.org/v1/electricity-generation/monthly?entity={entity}&start_date={startDate}&end_date={endDate}&api_key={apiKey}";
            if (!string.IsNullOrEmpty(series))
            {
                generationApiUrl += $"&series={Uri.EscapeDataString(series)}";
            }

            // Fetch generation data
            var generationData = await FetchAndDeserializeAsync(generationApiUrl);
            if (generationData == null)
            {
                return new List<ElectricalData>();
            }

            // Remove any rows that represent demand from the generation data
            generationData = generationData
                .Where(x => !x.Series.Equals("Demand", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // 2. Build the demand API URL
            string demandApiUrl = $"https://api.ember-energy.org/v1/electricity-demand/monthly?entity_code={entityCode}&start_date={startDate}&end_date={endDate}&api_key={apiKey}";
            var demandData = await FetchAndDeserializeAsync(demandApiUrl);
            if (demandData == null || !demandData.Any())
            {
                Console.WriteLine("Warning: No demand data available.");
                return generationData; // Return generation data as-is
            }

            // 3. Merge: Update each generation record with the monthly demand
            foreach (var genRecord in generationData)
            {
                // Compare only the year and month of the dates
                var matchingDemand = demandData.FirstOrDefault(d =>
                    d.Date.Year == genRecord.Date.Year && d.Date.Month == genRecord.Date.Month);

                if (matchingDemand != null)
                {
                    genRecord.DemandTWh = matchingDemand.DemandTWh;
                }
            }

            return generationData;
        }

        private async Task<List<ElectricalData>?> FetchAndDeserializeAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: API returned status code {response.StatusCode}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent) || responseContent.Trim() == "0")
                {
                    Console.WriteLine("API returned no data.");
                    return new List<ElectricalData>();
                }

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Data != null)
                {
                    return apiResponse.Data.Select(d => new ElectricalData
                    {
                        Date = DateOnly.Parse(d.Date),
                        Entity = d.Entity,
                        Series = d.Series,
                        DemandTWh = d.DemandTWh,
                        GenerationTWh = d.GenerationTWh
                    }).ToList();
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Deserialization Error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }

            return new List<ElectricalData>();
        }
    }
}
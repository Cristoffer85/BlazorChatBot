namespace BlazorAIChatBot.Shared.Models
{
    /* 
    ApiResponse class needed because Json response looks like this:
        "data": [
            {
                "entity": "string",
                "entity_code": "string",
                "is_aggregate_entity": true,
                "date": "string",
                "series": "string",
                "demand_twh":,
                "generation_twh":
            }
        ]
    */

    public class ApiResponse
    {
        public List<ApiData> Data { get; set; } = [];
    }

    // ApiData class to deserialize individual data items
    public class ApiData
    {
        [System.Text.Json.Serialization.JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("entity")]
        public string Entity { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("series")]
        public string Series { get; set; } = string.Empty; // New property for series

        [System.Text.Json.Serialization.JsonPropertyName("demand_twh")]
        public double DemandTWh { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("generation_twh")]
        public double GenerationTWh { get; set; }
    }
}
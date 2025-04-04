namespace BlazorAIChatBot.Server.Models
{

    /* 
    ApiResponse class needed because Json response looks like this
        "data": [
                    {
                    "entity": "string",
                    "entity_code": "string",
                    "is_aggregate_entity": true,
                    "date": "string",
                    "demand_twh": 0
                    }
                ]
    */

    public class ApiResponse
    {
        public List<ApiData> Data { get; set; } = new();
    }

    // Then ApiResponse class uses this ApiData class to deserialize the data
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
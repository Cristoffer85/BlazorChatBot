using BlazorAIChatBot.Shared.BotTraining;
using BlazorAIChatBot.Shared.Models;

namespace BlazorAIChatBot.Server.Services
{
    public class ChatService(ElectricalDataService electricalDataService)
    {
        private readonly ElectricalDataService _electricalDataService = electricalDataService;

        public async Task<ChatResponse> HandleChatRequestAsync(string question, string apiKey)
        {
            // Parse the user's question to extract parameters
            var (entity, startDate, endDate) = BotTraining.ParseQuestion(question);

            if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return new ChatResponse
                {
                    Sender = "AI",
                    Text = "I couldn't extract valid parameters from your query. Please try again with a proper format."
                };
            }

            // Fetch data using ElectricalDataService
            var data = await _electricalDataService.FetchElectricalDataAsync(entity, startDate, endDate, apiKey);

            if (data == null || !data.Any())
            {
                return new ChatResponse
                {
                    Sender = "AI",
                    Text = "I couldn't find any relevant data for your query. Please try again with different parameters."
                };
            }

            // Generate a response based on the fetched data
            var responseText = BotTraining.GenerateResponseFromData(data);

            return new ChatResponse
            {
                Sender = "AI",
                Text = responseText
            };
        }
    }
}
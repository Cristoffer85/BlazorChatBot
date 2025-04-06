using BlazorAIChatBot.Shared.BotTraining;
using BlazorAIChatBot.Shared.Models;

namespace BlazorAIChatBot.Server.Services
{
    public class ChatService
    {
        private readonly ElectricalDataService _electricalDataService;

        public ChatService(ElectricalDataService electricalDataService)
        {
            _electricalDataService = electricalDataService;
        }

        public async Task<ChatMessage> HandleChatRequestAsync(string question, string apiKey)
        {
            // Parse the user's question to extract parameters
            var (entity, startDate, endDate) = BotTraining.ParseQuestion(question);

            if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return new ChatMessage
                {
                    Sender = "AI",
                    Text = "I couldn't extract valid parameters from your query. Please try again with a proper format."
                };
            }

            try
            {
                // Fetch data using ElectricalDataService
                var data = await _electricalDataService.FetchElectricalDataAsync(entity, startDate, endDate, apiKey);

                if (data == null || !data.Any())
                {
                    return new ChatMessage
                    {
                        Sender = "AI",
                        Text = $"I couldn't find any data for {entity} from {startDate} to {endDate}. Please check your query or try a different time period."
                    };
                }

                // Generate a response based on the fetched data
                var responseText = BotTraining.GenerateResponseFromData(data, entity, startDate, endDate);

                return new ChatMessage
                {
                    Sender = "AI",
                    Text = responseText
                };
            }
            catch (Exception ex)
            {
                return new ChatMessage
                {
                    Sender = "AI",
                    Text = $"An error occurred while fetching data: {ex.Message}"
                };
            }
        }
    }
}
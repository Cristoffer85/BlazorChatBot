namespace BlazorAIChatBot.Shared.Models
{
    public class ChatResponse
    {
        public string Sender { get; set; } = "AI"; // Default sender is "AI"
        public string Text { get; set; } = string.Empty; // The response text
    }
}
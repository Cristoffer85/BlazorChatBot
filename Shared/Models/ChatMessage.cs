namespace BlazorAIChatBot.Shared.Models
{
    public class ChatMessage
    {
        public int Id { get; set; } // Primary Key
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
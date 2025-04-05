using Microsoft.AspNetCore.Mvc;
using BlazorAIChatBot.Server.Services;
using BlazorAIChatBot.Shared.Models;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatbotController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return Ok(new ChatResponse
            {
                Sender = "AI",
                Text = "I didn't understand your query. Please ask something like 'Show me data for Germany from 2023-01 to 2023-12.'"
            });
        }

        // Retrieve the API key
        var apiKey = Environment.GetEnvironmentVariable("EMBER_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            return StatusCode(500, new ChatResponse
            {
                Sender = "AI",
                Text = "The server is not configured properly. Please contact support."
            });
        }

        // Delegate the request to ChatService
        var response = await _chatService.HandleChatRequestAsync(request.Question, apiKey);

        return Ok(response);
    }
}
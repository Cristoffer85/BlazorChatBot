using BlazorAIChatBot.Server.Data;
using BlazorAIChatBot.Server.Services;
using BlazorAIChatBot.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAIChatBot.Server.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotDbContext _dbContext;
        private readonly ChatService _chatService;

        public ChatbotController(ChatbotDbContext dbContext, ChatService chatService)
        {
            _dbContext = dbContext;
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] ChatMessage message)
        {
            if (message == null) return BadRequest();

            // Save the user message to the database
            _dbContext.ChatMessages.Add(message);
            await _dbContext.SaveChangesAsync();

            // Retrieve the API key from environment variables
            string? apiKey = Environment.GetEnvironmentVariable("EMBER_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "API key is missing in the environment variables.");
            }

            // Process the user's query using ChatService
            var aiResponse = await _chatService.HandleChatRequestAsync(message.Text, apiKey);

            // Save the AI response to the database
            _dbContext.ChatMessages.Add(aiResponse);
            await _dbContext.SaveChangesAsync();

            return Ok(aiResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatHistory()
        {
            var chatHistory = await _dbContext.ChatMessages
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(chatHistory);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using BlazorAIChatBot.Shared.Models;

namespace BlazorAIChatBot.Server.Data
{
    public class ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : DbContext(options)
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
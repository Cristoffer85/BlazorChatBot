using BlazorAIChatBot.Server.Services;
using BlazorAIChatBot.Server.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load(@"c:\Projects\Csharp\BlazorAIChatBot\.env");

// Retrieve the database password and connection string
var dbPassword = DotNetEnv.Env.GetString("MSSQL_PASSWORD") 
    ?? throw new InvalidOperationException("Database password (MSSQL_PASSWORD) is not set in the .env file.");

var connectionString = builder.Configuration.GetConnectionString("ChatbotDatabase")?
    .Replace("{MSSQL_PASSWORD}", dbPassword) 
    ?? throw new InvalidOperationException("Connection string 'ChatbotDatabase' is not found in appsettings.json.");

// Configure services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ElectricalDataService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddDbContext<ChatbotDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
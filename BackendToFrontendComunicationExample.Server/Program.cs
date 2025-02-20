using BackendToFrontendComunicationExample.Server.Services;
using BackendToFrontendComunicationExample.Server.SignalRConfigurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:56670")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); // Important for SignalR
        });
});

builder.Services.AddHostedService<EventBackgroundService>();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowReactApp");

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.MapHub<EntryEventHub>("/eventEntryHub");
app.MapFallbackToFile("/index.html");

app.Run();

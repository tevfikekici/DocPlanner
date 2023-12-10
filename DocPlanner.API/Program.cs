using DocPlanner.API;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
builder.Services.AddHttpClient<ISlotService, SlotService>(client =>
{
    client.BaseAddress = new Uri("https://draliatest.azurewebsites.net/api/availability/");
    // Add basic authorization header
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("techuser:secretpassWord")));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Slot Booking API", Version = "v1" });
});
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/api.txt", rollingInterval: RollingInterval.Day));


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


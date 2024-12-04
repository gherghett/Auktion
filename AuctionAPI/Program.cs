using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;
using AuctionAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<AuctionService>();

builder.Services.AddDbContext<AuctionContext>(options =>
    options.UseSqlite("Data Source=auktion.db"));

// Register the AuctionFinalizer as a hosted service
builder.Services.AddHostedService<AuctionFinalizer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();
    AuctionContextSeeder.Seed(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

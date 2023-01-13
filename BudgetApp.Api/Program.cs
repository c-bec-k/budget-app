using BudgetApp;
using BudgetApp.SqlDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<BudgetAppDbContext>();
builder.Services.AddDbContext<BudgetAppDbContext>((options) =>
{
  var cs = builder.Configuration.GetConnectionString("BudgetSqlConnection");
  options.UseNpgsql(cs);
});

builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
  options.ClientId = ConfigurationPath.GetSectionKey("MicrosoftLogin.ClientId");
  options.ClientSecret = ConfigurationPath.GetSectionKey("MicrosoftLogin.ClientSecret");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger(new SwaggerOptions());
  app.UseSwaggerUI();
}

//app.UseAuthorization();
//app.UseAuthentication();

app.MapControllers();

app.MapGet("/", () => "Hello, Budget app!");

app.Run();

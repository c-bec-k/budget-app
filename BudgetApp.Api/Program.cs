﻿using BudgetApp;
using BudgetApp.SqlDbServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUser, SQLUserData>();
builder.Services.AddDbContext<BudgetAppDbContext>( (options) =>
{
  var cs = builder.Configuration.GetConnectionString("BudgetSqlConnection");
  options.UseNpgsql(cs);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger( new SwaggerOptions());
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Reflection;
using Api.Services;
using Application.Handlers;
using Domain.Services;
using Infra.Data;
using Infra.Data.Services;
using Infra.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConnectRabbit();
builder.Services.AddMediatRServices();
builder.Services.AddRabbitMqService();
builder.Services.AddHostedService<ProductLogService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
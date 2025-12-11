using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trilha_net_fundamentos_desafio.Context;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VeiculoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

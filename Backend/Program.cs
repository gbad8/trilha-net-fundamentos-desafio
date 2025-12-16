using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trilha_net_fundamentos_desafio.Context;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VeiculoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("PermitirFrontend");

app.MapControllers();

app.Run();

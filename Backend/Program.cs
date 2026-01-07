using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using trilha_net_fundamentos_desafio.Context;
using trilha_net_fundamentos_desafio.Services;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string" + "'DefaultConnection' not found.");

builder.Services.AddDbContext<VeiculoContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
    {
      options.AddDocumentTransformer((document, _, _) =>
          {
            document.Info = new OpenApiInfo
            {
              Title = "T&D Parking Management API",
              Version = "v1",
              Description = "API for managing a parking system.",

              Contact = new OpenApiContact
              {
                Name = "Guilherme d'Almeida",
                Email = "guilhermebarros181@gmail.com",
                Url = new Uri("https://github.com/gbad8")
              }
            };
            return Task.CompletedTask;
          });
    });

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowBlazorOrigin",
      policy =>
      {
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
      });
});

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference(options =>
      options.WithTitle("T&D Parking API")
             .WithFavicon("/favicon.svg"));
}

// app.UseHttpsRedirection();

app.UseCors("AllowBlazorOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

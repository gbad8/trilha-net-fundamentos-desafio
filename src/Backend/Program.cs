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

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddScoped<IPricesService, PricesService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
    {
      options.AddDocumentTransformer((document, _, _) =>
          {
            document.Info = new OpenApiInfo
            {
              Title = "T&D Parking Management API",
              Version = "v1",
              Description = "API for managing a parking system. Developed as a solution for the first project challange on the TIVIT Bootcamp with DIO.",

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VeiculoContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao migrar o banco de dados.");
    }
}

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference(options =>
      options.WithTitle("T&D Parking API")
             .WithFavicon("/favicon.svg")
             .WithTheme(ScalarTheme.Purple));
}

// app.UseHttpsRedirection();

app.UseCors("AllowBlazorOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

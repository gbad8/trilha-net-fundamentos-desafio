using Parking.Shared.Models;
using System.Net.Http.Json;

namespace newFrontend.Client.Services;

public class PricesService(HttpClient http) : IPricesService
{
  private readonly HttpClient _http = http;

  public async Task<List<Prices>> GetAllPricesAsync()
  {
    return await _http.GetFromJsonAsync<List<Prices>>("api/prices")
      ?? throw new InvalidOperationException("Nenhuma política de preços foi encontrada. Por favor, entre em contato com o administrator do sistema.");
  }

  public async Task<HttpResponseMessage> SetPriceAsync(int id, PriceToReadAndToSet newPrice)
  {
    return await _http.PatchAsJsonAsync($"api/prices/{id}", newPrice);
  }
}

using Parking.Shared.Models;
using System.Net.Http.Json;

namespace newFrontend.Client.Services;

public class PricesService(HttpClient http) : IPricesService
{
  private readonly HttpClient _http = http;

  public async Task<PriceToReadAndToSet> GetPriceAsync(int id)
  {
    return await _http.GetFromJsonAsync<PriceToReadAndToSet>($"api/prices/{id}")
      ?? throw new InvalidOperationException("Nenhuma política de preços foi encontrada para o tipo de veículo selecionado.");
  }
  public async Task<HttpResponseMessage> SetPriceAsync(int id, PriceToReadAndToSet newPrice)
  {
    return await _http.PatchAsJsonAsync($"api/prices/{id}", newPrice);
  }
}

using newFrontend.Client.Models;
using System.Net.Http.Json;

namespace newFrontend.Client.Services;

public class ParkingService(HttpClient http) : IParkingService
{
  private readonly HttpClient _http = http;

  public async Task<List<Veiculo>> GetVeiculosAsync()
  {
    return await _http.GetFromJsonAsync<List<Veiculo>>("api/veiculos") ?? new List<Veiculo>();
  }

  public async Task<Veiculo> CheckoutPreviewAsync(int id)
  {
    return await _http.GetFromJsonAsync<Veiculo>($"api/veiculos/{id}")
      ?? throw new InvalidOperationException("Veículo não encontrado na base de dados.\n Entre em contato com o administrador do sistema");
  }

  public async Task<HttpResponseMessage> MakeCheckoutAsync(int id, DateTime date)
  {
    return await _http.PatchAsJsonAsync($"api/veiculos/{id}", date);
  }
}

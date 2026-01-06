using Parking.Shared.Models;
using System.Net.Http.Json;

namespace newFrontend.Client.Services;

public class ParkingService(HttpClient http) : IParkingService
{
  private readonly HttpClient _http = http;

  public async Task<HttpResponseMessage> MakeCheckinAsync(VeiculoToCreate newVehicle)
  {
    return await _http.PostAsJsonAsync("api/veiculos/checkin", newVehicle);
  }

  public async Task<List<VeiculoToRead>> GetVeiculosAsync()
  {
    return await _http.GetFromJsonAsync<List<VeiculoToRead>>("api/veiculos/overview") ?? [];
  }

  public async Task<List<Veiculo>> GetHistoryAsync()
  {
    return await _http.GetFromJsonAsync<List<Veiculo>>("api/veiculos/history") ?? [];
  }

  public async Task<Veiculo> CheckoutPreviewAsync(int id)
  {
    return await _http.GetFromJsonAsync<Veiculo>($"api/veiculos/checkout-preview/{id}")
      ?? throw new InvalidOperationException("Veículo não encontrado na base de dados.\n Entre em contato com o administrador do sistema");
  }

  public async Task<HttpResponseMessage> MakeCheckoutAsync(VeiculoToUptade veiculoToCheckout)
  {
    return await _http.PatchAsJsonAsync($"api/veiculos/checkout", veiculoToCheckout);
  }

  public async Task<HttpResponseMessage> DeleteVehiclesFromHistory(HashSet<VeiculoToDelete> veiculosToDelete)
  {
    return await _http.PostAsJsonAsync("api/veiculos/delete", veiculosToDelete);
  }
}

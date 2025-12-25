using newFrontend.Client.Models;
using System.Net.Http.Json;

namespace newFrontend.Client.Services;

public class ParkingService : IParkingService
{
  private readonly HttpClient _http;

  public ParkingService(HttpClient http)
  {
    _http = http;
  }

  public async Task<List<Veiculo>> GetVeiculosAsync()
  {
    return await _http.GetFromJsonAsync<List<Veiculo>>("api/veiculos") ?? new List<Veiculo>();
  }
}

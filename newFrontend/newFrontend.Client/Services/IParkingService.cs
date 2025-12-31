using newFrontend.Client.Models;

namespace newFrontend.Client.Services;

public interface IParkingService
{
  Task<HttpResponseMessage> MakeCheckinAsync(Veiculo newVehicle);

  Task<List<Veiculo>> GetVeiculosAsync();

  Task<List<Veiculo>> GetHistoryAsync();

  Task<Veiculo> CheckoutPreviewAsync(int id);

  Task<HttpResponseMessage> MakeCheckoutAsync(int id, DateTime date);

  Task<HttpResponseMessage> DeleteVehiclesFromHistory(string id);

  string TranslateIdsToString(HashSet<Veiculo> selectedVehicles);
}

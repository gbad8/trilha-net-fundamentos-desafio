using Parking.Shared.Models;

namespace newFrontend.Client.Services;

public interface IParkingService
{
  Task<HttpResponseMessage> MakeCheckinAsync(VeiculoToCreate veiculo);

  Task<List<VeiculoToRead>> GetVeiculosAsync();

  Task<List<Veiculo>> GetHistoryAsync();

  Task<Veiculo> CheckoutPreviewAsync(int id);

  Task<HttpResponseMessage> MakeCheckoutAsync(VeiculoToUptade veiculoToCheckout);

  Task<HttpResponseMessage> DeleteVehiclesFromHistory(HashSet<VeiculoToDelete> veiculosToDelete);
}

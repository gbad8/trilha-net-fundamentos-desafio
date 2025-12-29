using newFrontend.Client.Models;

namespace newFrontend.Client.Services;

public interface IParkingService
{
  Task<List<Veiculo>> GetVeiculosAsync();

  Task<List<Veiculo>> GetHistoryAsync();

  Task<Veiculo> CheckoutPreviewAsync(int id);

  Task<HttpResponseMessage> MakeCheckoutAsync(int id, DateTime date);
}

using newFrontend.Client.Models;

namespace newFrontend.Client.Services;

public interface IParkingService
{
  Task<List<Veiculo>> GetVeiculosAsync();
}

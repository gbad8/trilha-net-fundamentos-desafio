using Parking.Shared.Models;
namespace trilha_net_fundamentos_desafio.Services;

public interface IParkingService
{
  void CheckingIn(Veiculo veiculo);
  void CheckingOut(Veiculo veiculo, DateTime date);
  decimal CalculateTicketPrice(Veiculo veiculo);
  VehicleType GetVehicleType(int id);

  Task<Veiculo> CheckinAsync(VeiculoToCreate newVeiculo);
  Task<IEnumerable<VeiculoToRead>> GetParkedVehiclesAsync();
  Task<IEnumerable<Veiculo>> GetVehicleHistoryAsync();
  Task<Veiculo?> GetVehicleByIdAsync(int id);
  Task CheckoutAsync(VeiculoToUptade veiculotoCheckout);
  Task DeleteVehiclesInBatchAsync(HashSet<VeiculoToDelete> veiculosToDelete);
}

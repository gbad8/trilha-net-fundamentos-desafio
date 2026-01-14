using Parking.Shared.Models;
namespace trilha_net_fundamentos_desafio.Services;

public interface IParkingService
{
  void CheckingIn(Veiculo veiculo);

  void CheckingOut(Veiculo veiculo, DateTime date);

  public decimal CalculateTicketPrice(Veiculo veiculo);

  public VehicleType GetVehicleType(int id);
}

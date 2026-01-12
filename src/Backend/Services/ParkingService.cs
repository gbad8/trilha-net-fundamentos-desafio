using Parking.Shared.Models;
namespace trilha_net_fundamentos_desafio.Services;

public class ParkingService(TimeProvider timeProvider) : IParkingService
{

  private readonly TimeProvider _timeProvider = timeProvider;

  public void CheckingIn(Veiculo veiculo)
  {
    veiculo.EntryTime = _timeProvider.GetLocalNow().DateTime;
    veiculo.DepartureTime = null;
    veiculo.TicketPrice = null;
  }

  public decimal GetPriceByVehicleType(Veiculo veiculo)
  {
    return veiculo.Type switch
    {
      VehicleType.Car => 10.00m,
      VehicleType.Motorcycle => 5.00m,
      _ => throw new NotImplementedException("O tipo de veículo não foi definido ainda pelo sistema.")
    };
  }

  public decimal CalculateTicketPrice(Veiculo veiculo)
  {
    DateTime departureTime = veiculo.DepartureTime ?? _timeProvider.GetLocalNow().DateTime;

    decimal hourlyPrice = GetPriceByVehicleType(veiculo);

    TimeSpan permanency = departureTime - veiculo.EntryTime;
    decimal totalHours = Math.Ceiling((decimal)permanency.TotalHours);

    return hourlyPrice * totalHours;
  }

  public void CheckingOut(Veiculo veiculo, DateTime checkoutTime)
  {
    if (veiculo.DepartureTime != null)
      throw new InvalidOperationException("Este veículo já realizou Checkout");

    veiculo.DepartureTime = checkoutTime;
    veiculo.TicketPrice = CalculateTicketPrice(veiculo);
  }
}

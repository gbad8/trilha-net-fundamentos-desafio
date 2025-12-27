using trilha_net_fundamentos_desafio.Models;

namespace trilha_net_fundamentos_desafio.Services;

public class ParkingService : IParkingService
{
  public void CheckingIn(Veiculo veiculo)
  {
    veiculo.EntryTime = DateTime.Now;
    veiculo.DepartureTime = null;
    veiculo.TicketPrice = null;
  }

  public void CheckingOut(Veiculo veiculo, DateTime checkoutTime)
  {
    if (veiculo.DepartureTime != null)
      throw new InvalidOperationException("Este veículo já realizou Checkout");

    veiculo.DepartureTime = checkoutTime;
    veiculo.TicketPrice = CalculateTicketPrice(veiculo);
  }

  public decimal CalculateTicketPrice(Veiculo veiculo)
  {
    DateTime departureTime = veiculo.DepartureTime ?? DateTime.Now;

    decimal hourlyPrice = (veiculo.Type == VehicleType.Car) ?
      10.00m : 5.00m;

    TimeSpan permanency = departureTime - veiculo.EntryTime;
    decimal totalHours = Math.Ceiling((decimal)permanency.TotalHours);

    return hourlyPrice * totalHours;
  }
}

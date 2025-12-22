using trilha_net_fundamentos_desafio.Models;

namespace trilha_net_fundamentos_desafio.Services;

public class ParkingService : IParkingService
{
  private const decimal HourlyPrice = 5.00m;

  public void CheckingIn(Veiculo veiculo)
  {
    veiculo.EntryTime = DateTime.Now;
    veiculo.DepartureTime = null;
    veiculo.TicketPrice = null;
  }

  public void CheckingOut(Veiculo veiculo)
  {
    if (veiculo.DepartureTime != null)
      throw new InvalidOperationException("Este veículo já realizou Checkout");

    veiculo.DepartureTime = DateTime.Now;
    TimeSpan permanency = veiculo.DepartureTime.Value - veiculo.EntryTime;

    if (permanency.TotalHours < 1)
      veiculo.TicketPrice = HourlyPrice;
    else
      veiculo.TicketPrice = HourlyPrice * (decimal)permanency.TotalHours;
  }
}

using Parking.Shared.Models;
namespace trilha_net_fundamentos_desafio.Services;

public class ParkingService(TimeProvider timeProvider) : IParkingService
{

  private readonly TimeProvider _timeProvider = timeProvider;

  public void CheckingIn(Veiculo veiculo)
  {
    veiculo.EntryTime = _timeProvider.GetLocalNow().DateTime;
    veiculo.DepartureTime = null;
    veiculo.EffectiveHourlyPrice = null;
    veiculo.TicketPrice = null;
  }

  public decimal CalculateTicketPrice(Veiculo veiculo)
  {
    if (veiculo.PricingPolicy == null)
      throw new InvalidOperationException("A política de preços não foi corretamente carregada.");

    decimal hourlyPrice = veiculo.PricingPolicy.Hourlyprice;

    DateTime departureTime = veiculo.DepartureTime ?? _timeProvider.GetLocalNow().DateTime;
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
    veiculo.EffectiveHourlyPrice = veiculo.PricingPolicy.Hourlyprice;
  }
}

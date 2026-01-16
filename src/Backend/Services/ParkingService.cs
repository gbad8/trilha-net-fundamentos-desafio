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

  public void CheckingOut(Veiculo veiculo, DateTime checkoutTime)
  {
    if (veiculo.DepartureTime != null)
      throw new InvalidOperationException("Este veículo já realizou Checkout");

    veiculo.DepartureTime = checkoutTime;
    veiculo.TicketPrice = CalculateTicketPrice(veiculo);
    veiculo.EffectiveHourlyPrice = veiculo.PricingPolicy.HourlyPrice;
  }

  public decimal CalculateTicketPrice(Veiculo veiculo)
  {
    if (veiculo.PricingPolicy == null)
      throw new InvalidOperationException("A política de preços não foi corretamente carregada.");

    decimal hourlyPrice = veiculo.PricingPolicy.HourlyPrice;

    DateTime departureTime = veiculo.DepartureTime ?? _timeProvider.GetLocalNow().DateTime;
    TimeSpan permanency = departureTime - veiculo.EntryTime;
    decimal totalHours = Math.Ceiling((decimal)permanency.TotalHours);

    return hourlyPrice * totalHours;
  }

  public VehicleType GetVehicleType(int id)
  {
    return id switch
    {
      0 => VehicleType.Car,
      1 => VehicleType.Motorcycle,
      _ => throw new ArgumentException("Não existe nenhuma política de preços para o veículo informado.")
    };
  }
}

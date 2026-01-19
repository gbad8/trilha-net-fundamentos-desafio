using Mapster;
using Microsoft.EntityFrameworkCore;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Context;

namespace trilha_net_fundamentos_desafio.Services;

public class ParkingService(TimeProvider timeProvider, VeiculoContext context) : IParkingService
{
  private readonly TimeProvider _timeProvider = timeProvider;
  private readonly VeiculoContext _context = context;

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

  public async Task<Veiculo> CheckinAsync(VeiculoToCreate newVeiculo)
  {
    var veiculo = newVeiculo.Adapt<Veiculo>();
    CheckingIn(veiculo);

    _context.Add(veiculo);
    await _context.SaveChangesAsync();

    return veiculo;
  }

  public async Task<IEnumerable<VeiculoToRead>> GetParkedVehiclesAsync()
  {
    var veiculos = await _context.Veiculos
                                 .Include(v => v.PricingPolicy)
                                 .Where(v => v.DepartureTime == null)
                                 .ToListAsync();

    foreach (Veiculo veiculo in veiculos)
    {
      veiculo.TicketPrice = CalculateTicketPrice(veiculo);
    }

    var veiculosToRead = veiculos.Adapt<List<VeiculoToRead>>();
    return veiculosToRead;
  }

  public async Task<IEnumerable<Veiculo>> GetVehicleHistoryAsync()
  {
    var veiculos = await _context.Veiculos
                                 .Include(v => v.PricingPolicy)
                                 .Where(v => v.DepartureTime != null)
                                 .ToListAsync();

    return veiculos;
  }

  public async Task<Veiculo?> GetVehicleByIdAsync(int id)
  {
    var veiculo = await _context.Veiculos
                                .Include(v => v.PricingPolicy)
                                .SingleOrDefaultAsync(v => v.Id == id);

    if (veiculo != null)
    {
      veiculo.DepartureTime = DateTime.Now;
      veiculo.TicketPrice = CalculateTicketPrice(veiculo);
    }

    return veiculo;
  }

  public async Task CheckoutAsync(VeiculoToUptade veiculoToCheckout)
  {
    var veiculoBanco = await _context.Veiculos
                                  .Include(v => v.PricingPolicy)
                                  .SingleAsync(v => v.Id == veiculoToCheckout.Id);

    CheckingOut(veiculoBanco, veiculoToCheckout.DepartureTime);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteVehiclesInBatchAsync(HashSet<VeiculoToDelete> veiculosToDelete)
  {
    if (veiculosToDelete == null || veiculosToDelete.Count == 0)
      throw new ArgumentException("Nenhum carro selecionado");

    var idsToDelete = veiculosToDelete.Select(v => v.Id).ToList();
    await _context.Veiculos.Where(v => idsToDelete.Contains(v.Id)).ExecuteDeleteAsync();
  }
}

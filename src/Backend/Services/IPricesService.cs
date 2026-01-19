using Parking.Shared.Models;

namespace trilha_net_fundamentos_desafio.Services;

public interface IPricesService
{
    Task<IEnumerable<Prices>> GetAllPricesAsync();
    Task UpdatePricesAsync(List<Prices> updatedPrices);
}
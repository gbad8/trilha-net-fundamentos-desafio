using Microsoft.EntityFrameworkCore;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Context;

namespace trilha_net_fundamentos_desafio.Services;

public class PricesService(VeiculoContext context) : IPricesService
{
    private readonly VeiculoContext _context = context;

    public async Task<IEnumerable<Prices>> GetAllPricesAsync()
    {
        return await _context.Prices.ToListAsync();
    }

    public async Task UpdatePricesAsync(List<Prices> updatedPrices)
    {
        foreach (var updatedPrice in updatedPrices)
        {
            var existingPrice = await _context.Prices.FindAsync(updatedPrice.Type);
            if (existingPrice != null)
            {
                existingPrice.HourlyPrice = updatedPrice.HourlyPrice;
            }
        }
        await _context.SaveChangesAsync();
    }
}
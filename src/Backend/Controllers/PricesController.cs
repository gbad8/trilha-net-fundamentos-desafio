using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Context;
using trilha_net_fundamentos_desafio.Services;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class PricesController(VeiculoContext context) : ControllerBase
{
  private readonly VeiculoContext _context = context;

  [Tags("Pricing Policy")]
  [EndpointName("GetAllPricingPolicy")]
  [EndpointSummary("Get all the prices")]
  [EndpointDescription("This endpoint gets the pricing policy for all registered vehicle types. It is called by the frontend, who iterates over the list creating cards with slides to show and to change the price.")]
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Prices>>> GetPrices()
  {
    return await _context.Prices.ToListAsync();
  }

  [Tags("Pricing Policy")]
  [EndpointName("SetPricingPolicy")]
  [EndpointSummary("Sets a new pricing policy")]
  [EndpointDescription("This endpoint updates the specified vehicle type with a new hourly price.")]
  [HttpPatch]
  public async Task<IActionResult> ChangePrices(List<Prices> updatedPrices)
  {
    try
    {
      foreach (var updatedPrice in updatedPrices)
      {
        var existingPrice = await _context.Prices.FindAsync(updatedPrice.Type);
        existingPrice?.HourlyPrice = updatedPrice.HourlyPrice;
      }
      await _context.SaveChangesAsync();
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest($"Erro ao atualizar preços: {ex.Message}");
    }
  }
}

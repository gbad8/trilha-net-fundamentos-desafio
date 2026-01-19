using Microsoft.AspNetCore.Mvc;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Services;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class PricesController(IPricesService pricesService) : ControllerBase
{
  private readonly IPricesService _pricesService = pricesService;

  [Tags("Pricing Policy")]
  [EndpointName("GetAllPricingPolicy")]
  [EndpointSummary("Get all the prices")]
  [EndpointDescription("This endpoint gets the pricing policy for all registered vehicle types. It is called by the frontend, who iterates over the list creating cards with slides to show and to change the price.")]
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Prices>>> GetPrices()
  {
    var prices = await _pricesService.GetAllPricesAsync();
    return Ok(prices);
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
      await _pricesService.UpdatePricesAsync(updatedPrices);
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest($"Erro ao atualizar preços: {ex.Message}");
    }
  }
}

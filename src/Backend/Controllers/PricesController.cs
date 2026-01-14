using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Context;
using trilha_net_fundamentos_desafio.Services;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class PricesController(VeiculoContext context, IParkingService service) : ControllerBase
{
  private readonly VeiculoContext _context = context;
  private readonly IParkingService _service = service;

  [Tags("Pricing Policy")]
  [EndpointName("GetAllPricingPolicy")]
  [EndpointSummary("Get all the prices")]
  [EndpointDescription("This endpoint gets the pricing policy for all registered vehicle types. It serves only for documentation purposes.")]
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Prices>>> GetPrices()
  {
    return await _context.Prices.ToListAsync();
  }

  [Tags("Pricing Policy")]
  [EndpointName("GetPricingPolicyByType")]
  [EndpointSummary("Gets price by type")]
  [EndpointDescription("This endpoint returns the pricing policy for the specified vehicle type")]
  [HttpGet("{id}")]
  public async Task<ActionResult<PriceToReadAndToSet>> GetPricesByType(int id)
  {
    var typeToGet = _service.GetVehicleType(id);
    var prices = await _context.Prices.FindAsync(typeToGet);

    if (prices == null)
    {
      return NotFound("Nenhuma política de preços foi encontrada para o tipo de veículo selecionado.");
    }

    var priceDTO = prices.Adapt<PriceToReadAndToSet>();

    return priceDTO;
  }

  [Tags("Pricing Policy")]
  [EndpointName("SetPricingPolicy")]
  [EndpointSummary("Sets a new pricing policy")]
  [EndpointDescription("This endpoint updates the specified vehicle type with a new hourly price.")]
  [HttpPatch("{id}")]
  public async Task<IActionResult> ChangePrices(int id, PriceToReadAndToSet price)
  {
    var typeToChange = _service.GetVehicleType(id);
    var pricingPolicyToChange = await _context.Prices.FindAsync(typeToChange);

    if (pricingPolicyToChange is not null && price.HourlyPrice != 0)
    {
      pricingPolicyToChange.HourlyPrice = price.HourlyPrice;
      await _context.SaveChangesAsync();

      return NoContent();
    }
    else
    {
      return NotFound("Nenhuma política de preços foi encontrada para o tipo de veículo selecionado.");
    }
  }
}

using Microsoft.AspNetCore.Mvc;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Context;
using Microsoft.EntityFrameworkCore;
using trilha_net_fundamentos_desafio.Services;
using Mapster;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class VeiculosController(VeiculoContext context, IParkingService service) : ControllerBase
{
  private readonly VeiculoContext _context = context;
  private readonly IParkingService _service = service;

  [Tags("Make Check-in")]
  [EndpointName("MakeCheckin")]
  [EndpointSummary("Check-in for the vehicles")]
  [EndpointDescription("This endpoint creates new vehicles in the database. The main page of the application has a button that calls a popup where the user can input the information for the new vehicle.")]
  [ProducesResponseType<Veiculo>(StatusCodes.Status201Created)]
  [HttpPost("checkin")]
  public async Task<IActionResult> Checkin(VeiculoToCreate newVeiculo)
  {
    var veiculo = newVeiculo.Adapt<Veiculo>();

    _service.CheckingIn(veiculo);
    _context.Add(veiculo);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetVehicleById), new { id = veiculo.Id }, veiculo);
  }

  [Tags("Parked Vehicles")]
  [EndpointName("GetVeiculos")]
  [EndpointSummary("Lists parked vehicles")]
  [EndpointDescription("This endpoint returns a list containing all the parked vehicles. The main page sends a request to this endppoint on initialization and populates a table with the vehicles data.")]
  [HttpGet("overview")]
  public async Task<ActionResult<IEnumerable<VeiculoToRead>>> GetParkedVehicles()
  {
    var veiculos = await _context.Veiculos
                                 .Where(x => x.DepartureTime == null)
                                 .ToListAsync();


    foreach (Veiculo veiculo in veiculos)
    {
      veiculo.TicketPrice = _service.CalculateTicketPrice(veiculo);
    }

    var veiculosToRead = veiculos.Adapt<List<VeiculoToRead>>();

    return veiculosToRead;
  }

  [Tags("History of Parked Vehicles")]
  [EndpointName("GetHistory")]
  [EndpointSummary("Lists all vehicles that used the parking service in the past")]
  [EndpointDescription("This endpoint returns a list containing all the vehicles that someday used the parking service. The history page sends a request to this endppoint on initialization and populates a table with the vehicles data.")]
  [HttpGet("history")]
  public async Task<ActionResult<IEnumerable<Veiculo>>> SearchHistory()
  {
    var veiculos = await _context.Veiculos
                                 .Where(x => x.DepartureTime != null)
                                 .ToListAsync();

    return veiculos;
  }

  [Tags("Make Check-out")]
  [EndpointName("CheckoutPreview")]
  [EndpointSummary("Gets the data for confirmation at checkout")]
  [EndpointDescription("This endpoints returns data for a single vehicle selected on the main page for checkout. The purpose is to give the operator all the useful information for charging the vehicle's owner.")]
  [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Requested vehicle was not found in the parking.")]
  [HttpGet("checkout-preview/{id}")]
  public async Task<ActionResult<Veiculo>> GetVehicleById(int id)
  {
    var veiculo = await _context.Veiculos.FindAsync(id);
    if (veiculo == null)
      return NotFound();

    veiculo.DepartureTime = DateTime.Now;
    veiculo.TicketPrice = _service.CalculateTicketPrice(veiculo);

    return veiculo;
  }

  [Tags("Make Check-out")]
  [EndpointName("MakeCheckout")]
  [EndpointSummary("Check-out for the vehicles")]
  [EndpointDescription("After confirming the vehicle's data, the operator clicks the confirmation button, that sends a request to this endpoint with the vehicle for checkout")]
  [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Requested vehicle was not found in the parking.")]
  [HttpPatch("checkout")]
  public async Task<IActionResult> Checkout(VeiculoToUptade veiculotoCheckout)
  {
    var veiculoBanco = await _context.Veiculos.FindAsync(veiculotoCheckout.Id);

    if (veiculoBanco == null)
      return NotFound();

    try
    {
      _service.CheckingOut(veiculoBanco, veiculotoCheckout.DepartureTime);
      await _context.SaveChangesAsync();
      return NoContent();
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ex.Message);
    }

  }

  [Tags("History of Parked Vehicles")]
  [EndpointName("DeleteVehiclesFromHistory")]
  [EndpointSummary("Deletes a vehicle from the database")]
  [EndpointDescription("On the history page, the user can select many vehicles he wants and click the deletion button. This button sends a request to this endpoint with the vehicles for exclusion.")]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "No vehicle selected.")]
  [HttpPost("delete")]
  public async Task<IActionResult> DeleteVehiclesInBatch(HashSet<VeiculoToDelete> veiculosToDelete)
  {
    if (veiculosToDelete == null || veiculosToDelete.Count == 0)
      return BadRequest("Nenhum carro selecionado");

    try
    {
      var idsToDelete = veiculosToDelete.Select(v => v.Id).ToList();

      await _context.Veiculos.Where(v => idsToDelete.Contains(v.Id)).ExecuteDeleteAsync();
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}

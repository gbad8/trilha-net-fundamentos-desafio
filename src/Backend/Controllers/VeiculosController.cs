using Microsoft.AspNetCore.Mvc;
using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Services;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class VeiculosController(IParkingService service) : ControllerBase
{
  private readonly IParkingService _service = service;

  [Tags("Make Check-in")]
  [EndpointName("MakeCheckin")]
  [EndpointSummary("Check-in for the vehicles")]
  [EndpointDescription("This endpoint creates new vehicles in the database. The main page of the application has a button that calls a popup where the user can input the information for the new vehicle.")]
  [ProducesResponseType<Veiculo>(StatusCodes.Status201Created)]
  [HttpPost("checkin")]
  public async Task<IActionResult> Checkin(VeiculoToCreate newVeiculo)
  {
    try
    {
      var veiculo = await _service.CheckinAsync(newVeiculo);
      return CreatedAtAction(nameof(GetVehicleById), new { id = veiculo.Id }, veiculo);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Tags("Parked Vehicles")]
  [EndpointName("GetVeiculos")]
  [EndpointSummary("Lists parked vehicles")]
  [EndpointDescription("This endpoint returns a list containing all the parked vehicles. The main page sends a request to this endppoint on initialization and populates a table with the vehicles data.")]
  [HttpGet("overview")]
  public async Task<ActionResult<IEnumerable<VeiculoToRead>>> GetParkedVehicles()
  {
    try
    {
      var veiculosToRead = await _service.GetParkedVehiclesAsync();
      return Ok(veiculosToRead);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Tags("History of Parked Vehicles")]
  [EndpointName("GetHistory")]
  [EndpointSummary("Lists all vehicles that used the parking service in the past")]
  [EndpointDescription("This endpoint returns a list containing all the vehicles that someday used the parking service. The history page sends a request to this endppoint on initialization and populates a table with the vehicles data.")]
  [HttpGet("history")]
  public async Task<ActionResult<IEnumerable<Veiculo>>> SearchHistory()
  {
    try
    {
      var veiculos = await _service.GetVehicleHistoryAsync();
      return Ok(veiculos);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Tags("Make Check-out")]
  [EndpointName("CheckoutPreview")]
  [EndpointSummary("Gets the data for confirmation at checkout")]
  [EndpointDescription("This endpoints returns data for a single vehicle selected on the main page for checkout. The purpose is to give the operator all the useful information for charging the vehicle's owner.")]
  [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Requested vehicle was not found in the parking.")]
  [HttpGet("checkout-preview/{id}")]
  public async Task<ActionResult<Veiculo>> GetVehicleById(int id)
  {
    try
    {
      var veiculo = await _service.GetVehicleByIdAsync(id);

      if (veiculo == null)
        return NotFound();

      return Ok(veiculo);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Tags("Make Check-out")]
  [EndpointName("MakeCheckout")]
  [EndpointSummary("Check-out for the vehicles")]
  [EndpointDescription("After confirming the vehicle's data, the operator clicks the confirmation button, that sends a request to this endpoint with the vehicle for checkout")]
  [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Requested vehicle was not found in the parking.")]
  [HttpPatch("checkout")]
  public async Task<IActionResult> Checkout(VeiculoToUptade veiculotoCheckout)
  {
    try
    {
      await _service.CheckoutAsync(veiculotoCheckout);
      return NoContent();
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
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
    try
    {
      await _service.DeleteVehiclesInBatchAsync(veiculosToDelete);
      return NoContent();
    }
    catch (ArgumentException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}

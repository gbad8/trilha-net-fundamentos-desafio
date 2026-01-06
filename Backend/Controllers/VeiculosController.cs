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

  [HttpPost]
  public async Task<IActionResult> Checkin(VeiculoToCreate newVeiculo)
  {
    var veiculo = newVeiculo.Adapt<Veiculo>();

    _service.CheckingIn(veiculo);
    _context.Add(veiculo);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetVehicleById), new { id = veiculo.Id }, veiculo);
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Veiculo>>> GetParkedVehicles()
  {
    var veiculos = await _context.Veiculos
                                 .Where(x => x.DepartureTime == null)
                                 .ToListAsync();

    foreach (Veiculo veiculo in veiculos)
    {
      veiculo.TicketPrice = _service.CalculateTicketPrice(veiculo);
    }

    return veiculos;
  }

  [HttpGet("history")]
  public async Task<ActionResult<IEnumerable<Veiculo>>> SearchHistory()
  {
    var veiculos = await _context.Veiculos
                                 .Where(x => x.DepartureTime != null)
                                 .ToListAsync();

    return veiculos;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Veiculo>> GetVehicleById(int id)
  {
    var veiculo = await _context.Veiculos.FindAsync(id);
    if (veiculo == null)
      return NotFound();

    veiculo.DepartureTime = DateTime.Now;
    veiculo.TicketPrice = _service.CalculateTicketPrice(veiculo);

    return veiculo;
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<Veiculo>> Checkout(int id, [FromBody] DateTime checkotTime)
  {
    var veiculoBanco = await _context.Veiculos.FindAsync(id);
    if (veiculoBanco == null)
      return NotFound();

    try
    {
      _service.CheckingOut(veiculoBanco, checkotTime);
      await _context.SaveChangesAsync();
      return veiculoBanco;
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ex.Message);
    }

  }

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

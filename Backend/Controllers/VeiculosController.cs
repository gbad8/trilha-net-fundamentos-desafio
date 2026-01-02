using Microsoft.AspNetCore.Mvc;
using trilha_net_fundamentos_desafio.Models;
using trilha_net_fundamentos_desafio.Context;
using Microsoft.EntityFrameworkCore;
using trilha_net_fundamentos_desafio.Services;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class VeiculosController(VeiculoContext context, IParkingService service) : ControllerBase
{
  private readonly VeiculoContext _context = context;
  private readonly IParkingService _service = service;

  [HttpPost]
  public async Task<IActionResult> Checkin(Veiculo veiculo)
  {
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

  [HttpDelete("Delete")]
  public async Task<IActionResult> DeleteVehiclesInBatch(string ids)
  {
    // Mantaining split and parse here only for test. Implementing a DTO soon.
    if (string.IsNullOrWhiteSpace(ids))
      return BadRequest("Nenhum id fornecido");

    try
    {
      var idList = ids.Split(',')
                      .Select(id => int.Parse(id.Trim()))
                      .ToList();

      var vehiclesToDelete = await _context.Veiculos
                                         .Where(v => idList.Contains(v.Id))
                                         .ToListAsync();

      if (vehiclesToDelete.Count == 0)
        return NotFound("Os veículos não foram encontrados no banco de dados.");

      _context.Veiculos.RemoveRange(vehiclesToDelete);
      await _context.SaveChangesAsync();
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}

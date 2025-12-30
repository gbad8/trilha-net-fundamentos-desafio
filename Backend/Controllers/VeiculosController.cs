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
  public async Task<IActionResult> GetParkedVehicles()
  {
    var veiculos = await _context.Veiculos
                                 .Where(x => x.DepartureTime == null)
                                 .ToListAsync();

    foreach (Veiculo veiculo in veiculos)
    {
      veiculo.TicketPrice = _service.CalculateTicketPrice(veiculo);
    }

    return Ok(veiculos);
  }

  [HttpGet("history")]
  public async Task<IActionResult> SearchHistory()
  {
    var veiculos = await _context.Veiculos
                                 .Where(x => x.DepartureTime != null)
                                 .ToListAsync();

    return Ok(veiculos);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetVehicleById(int id)
  {
    var veiculo = await _context.Veiculos.FindAsync(id);
    if (veiculo == null)
      return NotFound();

    veiculo.DepartureTime = DateTime.Now;
    veiculo.TicketPrice = _service.CalculateTicketPrice(veiculo);

    return Ok(veiculo);
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> Checkout(int id, [FromBody] DateTime checkotTime)
  {
    var veiculoBanco = await _context.Veiculos.FindAsync(id);
    if (veiculoBanco == null)
      return NotFound();

    try
    {
      _service.CheckingOut(veiculoBanco, checkotTime);
      await _context.SaveChangesAsync();
      return Ok(veiculoBanco);
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var veiculoBanco = await _context.Veiculos.FindAsync(id);
    if (veiculoBanco == null)
      return NotFound("Veiculo não encontrado no banco de dados");

    _context.Veiculos.Remove(veiculoBanco);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  [HttpDelete("batchDelete")]
  public async Task<IActionResult> DeleteVehiclesInBatch([FromQuery] string ids)
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
        return NotFound("Os veículos não encontrados no banco de dados.");

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

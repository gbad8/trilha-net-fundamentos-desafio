using Microsoft.AspNetCore.Mvc;
using trilha_net_fundamentos_desafio.Models;
using trilha_net_fundamentos_desafio.Context;
using Microsoft.EntityFrameworkCore;
using trilha_net_fundamentos_desafio.Services;

namespace trilha_net_fundamentos_desafio.Controllers;

[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
  private readonly VeiculoContext _context;
  private readonly IParkingService _service;

  public VeiculosController(VeiculoContext context, IParkingService service)
  {
    _context = context;
    _service = service;
  }

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
  public IActionResult Delete(int id)
  {
    var veiculoBanco = _context.Veiculos.Find(id);
    if (veiculoBanco == null)
      return NotFound();

    _context.Veiculos.Remove(veiculoBanco);
    _context.SaveChanges();

    return NoContent();
  }
}

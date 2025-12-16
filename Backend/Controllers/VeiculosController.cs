using Microsoft.AspNetCore.Mvc;
using trilha_net_fundamentos_desafio.Models;
using trilha_net_fundamentos_desafio.Context;

namespace trilha_net_fundamentos_desafio.Controllers;

    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
      private readonly VeiculoContext _context;

      public VeiculosController(VeiculoContext context)
      {
        _context = context;
      }

      [HttpPost]
      public IActionResult Create(Veiculo veiculo)
      {
        _context.Add(veiculo);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Read), new { id = veiculo.Id}, veiculo); 
      }

      [HttpGet("{id}")]
      public IActionResult Read(int id)
      {
        var veiculo = _context.Veiculos.Find(id);
        if (veiculo == null)
          return NotFound();

        return Ok(veiculo);
      }

      [HttpGet("get/{placa?}")]
      public IActionResult ReadByPlaca(string? placa)
        {
          if (string.IsNullOrEmpty(placa))
          {
            var veiculos = _context.Veiculos.ToList();
            return Ok(veiculos);
          }
          else 
          {
            var veiculos = _context.Veiculos.Where(x => x.Placa.Contains(placa));
            return Ok(veiculos);
          }
        }
      
      [HttpGet]
      public IActionResult ReadAll()
      {
        var veiculos = _context.Veiculos.ToList();
        return Ok(veiculos);
      }

      [HttpPut("{id}")]
      public IActionResult Update(int id, Veiculo veiculo)
      {
        var veiculoBanco = _context.Veiculos.Find(id);
        if(veiculoBanco == null)
          return NotFound();

        veiculoBanco.Placa = veiculo.Placa;
        veiculoBanco.HorasEstacionado = veiculo.HorasEstacionado;

        _context.Veiculos.Update(veiculoBanco);
        _context.SaveChanges();

        return Ok(veiculoBanco);
      }

      [HttpDelete("{id}")]
      public IActionResult Delete(int id)
      {
        var veiculoBanco = _context.Veiculos.Find(id);
        if(veiculoBanco == null)
          return NotFound();

        _context.Veiculos.Remove(veiculoBanco);
        _context.SaveChanges();

        return NoContent();
      }
    }

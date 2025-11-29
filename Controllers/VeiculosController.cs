using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using trilha_net_fundamentos_desafio.Models;

namespace trilha_net_fundamentos_desafio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
      static private List<Veiculo> veiculos = new List<Veiculo>
      {
        new()
        {
          Id = 1,
          Placa = "ABCD-1234",
          HorasEstacionado = 5
        },
        new()
        {
          Id = 2,
          Placa = "QWER-1234",
          HorasEstacionado = 3
        },
        new()
        {
          Id = 3,
          Placa = "DVOR-1234",
          HorasEstacionado = 6
        }
      };

      [HttpGet]
      public ActionResult<List<Veiculo>> GetVeiculos()
      {
        return Ok(veiculos);
      }

      [HttpGet("{id}")]
      public ActionResult<Veiculo> GetVeiculoById(int id) 
      {
        var veiculo = veiculos.FirstOrDefault(x => x.Id == id);
        if (veiculo == null)
          return NotFound();

        return Ok(veiculo);
      }

      [HttpPost]
      public ActionResult<Veiculo> AddVeiculo(Veiculo veiculoNovo)
      {
        if(veiculoNovo == null)
          return BadRequest();
        
        veiculos.Add(veiculoNovo);

        return CreatedAtAction(
            nameof(GetVeiculoById),
            new {id = veiculoNovo.Id, veiculoNovo});
      }

      [HttpPut("{id}")]
      public IActionResult AtualizarVeiculo(int id, Veiculo veiculoAtualizado)
      {
        var veiculo = veiculos.FirstOrDefault(x => x.Id == id);
        if (veiculo == null)
          return NotFound();

        veiculo.Id = veiculoAtualizado.Id;
        veiculo.Placa = veiculoAtualizado.Placa;
        veiculo.HorasEstacionado = veiculoAtualizado.HorasEstacionado;

        return NoContent();
      }

      [HttpDelete("{id}")]
      public IActionResult DeletarVeiculo(int id)
      {
        var veiculo = veiculos.FirstOrDefault(x => x.Id == id);
        if (veiculo == null)
          return NotFound();

        veiculos.Remove(veiculo);
        return NoContent();
      }
    }
}

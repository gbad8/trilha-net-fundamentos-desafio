using Microsoft.EntityFrameworkCore;
using trilha_net_fundamentos_desafio.Models;

namespace trilha_net_fundamentos_desafio.Context;

public class VeiculoContext : DbContext
{
  public VeiculoContext(DbContextOptions<VeiculoContext> options) : base(options)
  {

  }

  public DbSet<Veiculo> Veiculos { get; set; }
}

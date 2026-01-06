using Microsoft.EntityFrameworkCore;
using Parking.Shared.Models;

namespace trilha_net_fundamentos_desafio.Context;

public class VeiculoContext(DbContextOptions<VeiculoContext> options) : DbContext(options)
{
  public DbSet<Veiculo> Veiculos { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Veiculo>()
      .Property(v => v.TicketPrice)
      .HasColumnType("decimal(18,2)");
  }
}

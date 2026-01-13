using Microsoft.EntityFrameworkCore;
using Parking.Shared.Models;

namespace trilha_net_fundamentos_desafio.Context;

public class VeiculoContext(DbContextOptions<VeiculoContext> options) : DbContext(options)
{
  public DbSet<Veiculo> Veiculos { get; set; }
  public DbSet<Prices> Prices { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Veiculo>(entity =>
        {
          entity.Property(v => v.TicketPrice).HasColumnType("decimal(18,2)");
          entity.Property(v => v.EffectiveHourlyPrice).HasColumnType("decimal(18,2)");
        });

    modelBuilder.Entity<Prices>()
      .Property(p => p.Hourlyprice).HasColumnType("decimal(18,2)");

    modelBuilder.Entity<Veiculo>()
      .HasOne(v => v.PricingPolicy)
      .WithMany()
      .HasForeignKey(v => v.Type)
      .IsRequired();

    modelBuilder.Entity<Prices>().HasData(
        new Prices { Type = VehicleType.Car, Hourlyprice = 10.00m },
        new Prices { Type = VehicleType.Motorcycle, Hourlyprice = 5.00m }
        );
  }
}

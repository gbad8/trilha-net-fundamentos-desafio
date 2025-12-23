namespace trilha_net_fundamentos_desafio.Models;

public class Veiculo
{
  public int Id { get; set; }
  public required string Placa { get; set; }
  public required VehicleType Type { get; set; }

  public DateTime EntryTime { get; set; }
  public DateTime? DepartureTime { get; set; }

  public decimal? TicketPrice { get; set; }
}

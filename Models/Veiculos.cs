namespace trilha_net_fundamentos_desafio.Models;

public class Veiculo
{
  public int Id { get; set; }
  public required string Placa { get; set; }
  public int HorasEstacionado { get; set; }
}

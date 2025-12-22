using trilha_net_fundamentos_desafio.Models;

namespace trilha_net_fundamentos_desafio.Services;

interface IParkingService
{
  void CheckingIn(Veiculo veiculo);

  void CheckingOut(Veiculo veiculo);
}

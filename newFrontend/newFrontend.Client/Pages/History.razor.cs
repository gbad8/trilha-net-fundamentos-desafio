namespace newFrontend.Client.Pages;

using newFrontend.Client.Models;
using MudBlazor;

public partial class History
{
  private List<Veiculo> veiculos = [];
  private bool loading = true;
  private string searchString1 = "";
  private HashSet<Veiculo> selectedItems = [];
  readonly bool fixed_header = true;
  readonly bool fixed_footer = false;

  protected override async Task OnInitializedAsync()
  {
    try
    {
      veiculos = await ParkingService.GetHistoryAsync();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erro: {ex.Message}");
    }
    finally
    {
      loading = false;
    }
  }

  private static string FormatType(VehicleType tipo)
  {
    if (tipo == VehicleType.Motorcycle)
      return "Moto";

    return "Carro";
  }

  private static bool FilterFunc(Veiculo veiculo, string searchString)
  {
    if (string.IsNullOrWhiteSpace(searchString))
      return true;
    if (veiculo.Placa.Contains(searchString, StringComparison.OrdinalIgnoreCase))
      return true;
    return false;
  }

  private async Task DeleteVehicles(HashSet<Veiculo> itens)
  {
    var dialogReference = await DialogService.ShowAsync<ExclusionConfirmationDialog>("Confirmação de Exclusão");
    var result = await dialogReference.Result;

    if (result is not null)
    {
      if (!result.Canceled)
      {
        string stringIds = ParkingService.TranslateIdsToString(itens);
        string uri = string.Concat("?", "ids=", stringIds);
        var response = await ParkingService.DeleteVehiclesFromHistory(uri);

        if (response.IsSuccessStatusCode)
        {
          Snackbar.Add($"Veículo(s) excluídos com sucesso.", Severity.Success);
        }
        else
        {
          Snackbar.Add($"Erro ao excuir veículo.", Severity.Error);
        }
      }
    }
  }
}

namespace newFrontend.Client.Pages;

using Parking.Shared.Models;
using MudBlazor;
using Mapster;

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

  private async Task DeleteVehicles(HashSet<Veiculo> vehiclesToDelete)
  {
    var dialogReference = await DialogService.ShowAsync<ExclusionConfirmationDialog>("Confirmação de Exclusão");
    var result = await dialogReference.Result;

    if (result is not null)
    {
      if (!result.Canceled)
      {
        var vehiclesToDeleteDto = vehiclesToDelete.Adapt<HashSet<VeiculoToDelete>>();

        var response = await ParkingService.DeleteVehiclesFromHistory(vehiclesToDeleteDto);

        if (response.IsSuccessStatusCode)
        {
          Snackbar.Add($"Veículo(s) excluídos com sucesso.", Severity.Success);
          veiculos = await ParkingService.GetHistoryAsync();
        }
        else
        {
          Snackbar.Add($"Erro ao excuir veículo.", Severity.Error);
        }
      }
    }
  }
}

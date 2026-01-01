namespace newFrontend.Client.Pages;

using MudBlazor;
using newFrontend.Client.Models;

public partial class Home
{
  private List<Veiculo> veiculos = [];
  private bool loading = true;
  private string searchString1 = "";
  readonly bool fixed_header = true;
  readonly bool fixed_footer = false;

  protected override async Task OnInitializedAsync()
  {
    try
    {
      veiculos = await ParkingService.GetVeiculosAsync();
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

  private async Task MakeCheckin()
  {
    var dialogReference = await DialogService.ShowAsync<CheckinDialog>("Realizar Check-in");
    var result = await dialogReference.Result;

    if (result is not null && !result.Canceled)
    {
      if (result.Data is Veiculo newVehicle)
      {
        var response = await ParkingService.MakeCheckinAsync(newVehicle);
        if (response.IsSuccessStatusCode)
        {
          Snackbar.Add($"Check-in realizado com sucesso!", Severity.Success);
          veiculos = await ParkingService.GetVeiculosAsync();
        }
        else
        {
          Snackbar.Add($"Erro ao realizar check-in", Severity.Error);
        }
      }
    }
  }

  private async Task MakeCheckout(Veiculo veiculo)
  {
    var parameters = new DialogParameters { ["VehicleId"] = veiculo.Id };

    var dialogReference = await DialogService.ShowAsync<CheckoutDialog>("Realizar Check-out", parameters);
    var result = await dialogReference.Result;

    if (result is not null)
    {
      if (!result.Canceled)
      {
        if (result.Data is DateTime previewDate)
        {
          var response = await ParkingService.MakeCheckoutAsync(veiculo.Id, previewDate);
          if (response.IsSuccessStatusCode)
          {
            Snackbar.Add($"Check-out de {veiculo.Placa} realizado!", Severity.Success);
            veiculos = await ParkingService.GetVeiculosAsync();
          }
          else
          {
            Snackbar.Add("Erro ao realizar check-out.", Severity.Error);
          }
        }
      }
    }
  }
}


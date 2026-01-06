namespace newFrontend.Client.Pages;

using MudBlazor;
using Parking.Shared.Models;
using Mapster;

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
      var veiculosToRead = await ParkingService.GetVeiculosAsync();
      veiculos = veiculosToRead.Adapt<List<Veiculo>>();
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
      if (result.Data is VeiculoToCreate newVehicle)
      {
        var response = await ParkingService.MakeCheckinAsync(newVehicle);
        if (response.IsSuccessStatusCode)
        {
          Snackbar.Add($"Check-in realizado com sucesso!", Severity.Success);
          var veiculosToRead = await ParkingService.GetVeiculosAsync();
          veiculos = veiculosToRead.Adapt<List<Veiculo>>();
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
          var veiculoToCheckout = veiculo.Adapt<VeiculoToUptade>();
          var response = await ParkingService.MakeCheckoutAsync(veiculoToCheckout);
          if (response.IsSuccessStatusCode)
          {
            Snackbar.Add($"Check-out de {veiculo.Placa} realizado!", Severity.Success);
            var veiculosToRead = await ParkingService.GetVeiculosAsync();
            veiculos = veiculosToRead.Adapt<List<Veiculo>>();
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


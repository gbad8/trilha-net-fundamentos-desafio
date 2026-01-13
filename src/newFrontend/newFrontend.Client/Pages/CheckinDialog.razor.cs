namespace newFrontend.Client.Pages;

using Parking.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class CheckinDialog
{
  [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

  MudForm? form;
  bool Success;
  string[] Errors = [];

  private string? _plate;
  string? VehiclePlate
  {
    get => _plate;
    set
    {
      _plate = value?.ToUpper();
    }
  }

  VehicleType TypeOfTheVehicle { get; set; } = VehicleType.Car;

  private void Cancel() => MudDialog?.Cancel();

  private async Task Confirm()
  {
    await form!.Validate();

    if (form.IsValid)
    {
      VeiculoToCreate checkinVehicle = new(VehiclePlate!, TypeOfTheVehicle);

      MudDialog!.Close(DialogResult.Ok(checkinVehicle));
    }
  }
}

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
  string? VehiclePlate { get; set; }
  VehicleType TypeOfTheVehicle { get; set; } = VehicleType.Car;

  private void Cancel() => MudDialog?.Cancel();

  private async Task Confirm()
  {
    await form!.Validate();

    if (form.IsValid)
    {
      // Em breve implementarei DTOs
      Veiculo checkinData = new()
      {
        Placa = VehiclePlate!,
        Type = TypeOfTheVehicle
      };

      MudDialog!.Close(DialogResult.Ok(checkinData));
    }
  }
}

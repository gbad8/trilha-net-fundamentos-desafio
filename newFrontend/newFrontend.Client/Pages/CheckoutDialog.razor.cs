namespace newFrontend.Client.Pages;

using newFrontend.Client.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class CheckoutDialog
{
  [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

  [Parameter] public int VehicleId { get; set; }

  private Veiculo? vehiclepreview;

  protected override async Task OnInitializedAsync()
  {
    vehiclepreview = await ParkingService.CheckoutPreviewAsync(VehicleId);
  }

  private void Cancel() => MudDialog?.Cancel();

  private void Confirm()
  {
    if (vehiclepreview?.DepartureTime != null)
    {
      MudDialog?.Close(DialogResult.Ok(vehiclepreview.DepartureTime.Value));
    }
  }
}


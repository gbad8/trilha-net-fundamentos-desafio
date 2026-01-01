namespace newFrontend.Client.Pages;

using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class ExclusionConfirmationDialog
{
  [CascadingParameter] IMudDialogInstance? MudDialog { get; set; }

  private void Cancel() => MudDialog?.Cancel();

  private void Confirm()
  {
    MudDialog?.Close(DialogResult.Ok(true));
  }
}

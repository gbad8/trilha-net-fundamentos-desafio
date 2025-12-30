namespace newFrontend.Client.Pages;

using newFrontend.Client.Models;

public partial class History
{
  private List<Veiculo> veiculos = new();
  private bool loading = true;
  private string searchString1 = "";
  private HashSet<Veiculo> selectedItems = new HashSet<Veiculo>();
  bool fixed_header = true;
  bool fixed_footer = false;

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

  private string FormatType(VehicleType tipo)
  {
    if (tipo == VehicleType.Motorcycle)
      return "Moto";

    return "Carro";
  }

  private bool FilterFunc(Veiculo veiculo, string searchString)
  {
    if (string.IsNullOrWhiteSpace(searchString))
      return true;
    if (veiculo.Placa.Contains(searchString, StringComparison.OrdinalIgnoreCase))
      return true;
    return false;
  }
}

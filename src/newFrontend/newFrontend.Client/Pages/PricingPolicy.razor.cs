using System.Globalization;
using System.Reflection.Metadata;
using Parking.Shared.Models;

namespace newFrontend.Client.Pages;

public partial class PrincingPolicy
{
  private readonly CultureInfo culture = CultureInfo.DefaultThreadCurrentCulture!;
  private bool buttonDisabled = true;
  public decimal priceForCarUser;
  public decimal priceForMotorcycleUser;
  private List<Prices>? allPrices;

  protected override async Task OnInitializedAsync()
  {
    try
    {
      allPrices = await PricesService.GetAllPricesAsync();

      priceForCarUser = allPrices.FirstOrDefault(p => p.Type == VehicleType.Car)?.HourlyPrice ?? 0;
      priceForMotorcycleUser = allPrices.FirstOrDefault(p => p.Type == VehicleType.Motorcycle)?.HourlyPrice ?? 0;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erro: {ex.Message}");
    }
  }

  private decimal GetPriceValue(VehicleType type) => type switch
  {
    VehicleType.Car => priceForCarUser,
    VehicleType.Motorcycle => priceForMotorcycleUser,
    _ => 0
  };

  private static string GetVehicleName(VehicleType type) => type switch
  {
    VehicleType.Car => "Carros",
    VehicleType.Motorcycle => "Motocicletas",
    _ => "Desconhecido"
  };

  private void OnPriceChange(Prices pricingPolicy)
  {
    var originalPrice = GetPriceValue(pricingPolicy.Type);

    if (pricingPolicy.HourlyPrice != originalPrice)
      buttonDisabled = false;

    StateHasChanged();
  }
}

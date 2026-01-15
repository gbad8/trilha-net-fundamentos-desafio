using System.Globalization;
using Parking.Shared.Models;

namespace newFrontend.Client.Pages;

public partial class PrincingPolicy
{
  private readonly CultureInfo culture = CultureInfo.DefaultThreadCurrentCulture!;
  public decimal priceForCarUser;
  public decimal priceForMotorcycleUser;
  PriceToReadAndToSet? priceForCar;
  PriceToReadAndToSet? priceForMotorcycle;

  protected override async Task OnInitializedAsync()
  {
    try
    {
      Task<PriceToReadAndToSet> taskForCar = PricesService.GetPriceAsync((int)VehicleType.Car);
      Task<PriceToReadAndToSet> taskForMotorcycle = PricesService.GetPriceAsync((int)VehicleType.Motorcycle);

      await Task.WhenAll(taskForCar, taskForMotorcycle);

      priceForCar = await taskForCar;
      priceForMotorcycle = await taskForMotorcycle;

      priceForCarUser = priceForCar.HourlyPrice;
      priceForMotorcycleUser = priceForMotorcycle.HourlyPrice;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erro: {ex.Message}");
    }
  }

  public bool IsDisable()
  {
    return (priceForCarUser == priceForCar!.HourlyPrice)
      && (priceForMotorcycleUser == priceForMotorcycle!.HourlyPrice);
  }
}

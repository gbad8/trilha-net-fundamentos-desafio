using System.Globalization;

namespace newFrontend.Client.Pages;

public partial class PrincingPolicy
{
  private readonly CultureInfo culture = CultureInfo.DefaultThreadCurrentCulture!;
  public decimal value = 50.00m;
}

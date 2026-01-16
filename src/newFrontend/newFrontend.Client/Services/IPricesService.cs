using Parking.Shared.Models;

namespace newFrontend.Client.Services;

public interface IPricesService
{
  Task<List<Prices>> GetAllPricesAsync();
  Task<HttpResponseMessage> SetPriceAsync(int id, PriceToReadAndToSet newPrice);
}

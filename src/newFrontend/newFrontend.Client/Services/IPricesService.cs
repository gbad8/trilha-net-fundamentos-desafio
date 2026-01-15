using Parking.Shared.Models;

namespace newFrontend.Client.Services;

public interface IPricesService
{
  Task<PriceToReadAndToSet> GetPriceAsync(int id);
  Task<HttpResponseMessage> SetPriceAsync(int id, PriceToReadAndToSet newPrice);
}

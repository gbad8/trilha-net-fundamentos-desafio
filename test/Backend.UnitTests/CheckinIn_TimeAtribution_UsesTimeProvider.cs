using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Services;
using Microsoft.Extensions.Time.Testing;

namespace Backend.UnitTests;

public class UnitTest1
{
  [Fact]
  public void CheckingIn_TimeAtribution_UsesTimeProvider()
  {
    // setting the local time zone and getting the "now" time
    var stubTimeProvider = new FakeTimeProvider();
    stubTimeProvider.SetLocalTimeZone(TimeZoneInfo.Local);
    var fixedDate = stubTimeProvider.GetLocalNow();

    // mocking the vehicle and injecting the FakeTimeProvider on the service
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0 };
    var service = new ParkingService(stubTimeProvider);

    service.CheckingIn(mockVehicle);

    Assert.Equal(fixedDate, mockVehicle.EntryTime);
  }
}

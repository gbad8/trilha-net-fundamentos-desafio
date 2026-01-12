using Parking.Shared.Models;
using trilha_net_fundamentos_desafio.Services;
using Microsoft.Extensions.Time.Testing;

namespace Backend.UnitTests;

public class ParkinsServiceTests
{
  private readonly FakeTimeProvider stubTimeProvider;
  private readonly ParkingService service;

  public ParkinsServiceTests()
  {
    stubTimeProvider = new FakeTimeProvider();
    stubTimeProvider.SetLocalTimeZone(TimeZoneInfo.Local);
    service = new ParkingService(stubTimeProvider);
  }

  [Fact]
  public void CheckingIn_TimeAtribution_UsesTimeProvider()
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0 };
    var fixedDate = stubTimeProvider.GetLocalNow();

    service.CheckingIn(mockVehicle);

    Assert.Equal(fixedDate, mockVehicle.EntryTime);
  }

  [Theory]
  [InlineData(VehicleType.Car)]
  [InlineData(VehicleType.Motorcycle)]
  public void GetPriceByVehicleType_TestintAllTheTypes_ReturnCorrectValue(VehicleType testType)
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = testType };
    decimal correctPrice = testType switch
    {
      VehicleType.Car => 10.00m,
      VehicleType.Motorcycle => 5.00m,
      _ => throw new NotImplementedException("Tipo de veículo não implementado.")
    };

    var testPrice = service.GetPriceByVehicleType(mockVehicle);

    Assert.Equal(correctPrice, testPrice);
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  public void CalculateTicketPrice_DepartureTimeNotGiven_UsesTimeProvider(int hours)
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0 };
    mockVehicle.EntryTime = stubTimeProvider.GetLocalNow().DateTime;
    stubTimeProvider.Advance(TimeSpan.FromHours(hours));
    decimal expectedValue = service.GetPriceByVehicleType(mockVehicle) * hours;

    decimal testPrice = service.CalculateTicketPrice(mockVehicle);

    Assert.Equal(expectedValue, testPrice);
  }

  [Theory]
  [InlineData("10/10/2025 11:00:00 AM", "10")] // 1 hours
  [InlineData("10/10/2025 12:00:00 PM", "20")] // 2 hours
  [InlineData("10/10/2025 12:10:00 PM", "30")] // 3 hours
  public void CalculateTicketPrice_DepartureTimeGiven_ReturnCorrectValues(string date, string value)
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0 };
    mockVehicle.EntryTime = DateTime.Parse("10/10/2025 10:00:00 AM");
    mockVehicle.DepartureTime = DateTime.Parse(date);
    decimal expectedValue = decimal.Parse(value);

    mockVehicle.TicketPrice = service.CalculateTicketPrice(mockVehicle);

    Assert.Equal(expectedValue, mockVehicle.TicketPrice);
  }

  [Fact]
  public void CheckingOut_CheckoutTimeNotNull_ThrowsException()
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0 };
    mockVehicle.DepartureTime = DateTime.Parse("10/10/2025 10:00:00 AM"); // a ramdom time
    var stubDepartureTime = stubTimeProvider.GetLocalNow().DateTime;

    // the method is void, so i'm acting on the assertion
    var testingException = Assert.Throws<InvalidOperationException>(() =>
        {
          service.CheckingOut(mockVehicle, stubDepartureTime);
        });
  }

  [Theory]
  [InlineData("10/10/2025 11:00:00 AM")] // 1 hours
  [InlineData("10/10/2025 12:00:00 PM")] // 2 hours
  [InlineData("10/10/2025 12:10:00 PM")] // 3 hours
  public void CheckingOut_DepartureTimeNull_UsesPassedDepartureTime(string passedDepartureTime)
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0 };
    var departureTime = DateTime.Parse(passedDepartureTime);

    service.CheckingOut(mockVehicle, departureTime);

    Assert.Equal(departureTime, mockVehicle.DepartureTime);
  }
}

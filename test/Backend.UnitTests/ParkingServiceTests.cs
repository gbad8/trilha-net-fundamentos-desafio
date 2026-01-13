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

  [Fact]
  public void CalculateTicketPrice_PolicyPriceNotGiven_ThrowsAnExpception()
  {
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0, };
    mockVehicle.EntryTime = stubTimeProvider.GetLocalNow().DateTime;

    // the method is void, so the test acts on the assertion
    var testingException = Assert.Throws<InvalidOperationException>(() =>
        service.CalculateTicketPrice(mockVehicle));
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  public void CalculateTicketPrice_DepartureTimeNotGiven_UsesTimeProvider(int hoursInThePark)
  {
    var pricesTable = new Prices { Type = 0, Hourlyprice = 10.00m }; // a car
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0, PricingPolicy = pricesTable };
    mockVehicle.EntryTime = stubTimeProvider.GetLocalNow().DateTime;
    stubTimeProvider.Advance(TimeSpan.FromHours(hoursInThePark));
    decimal expectedValue = mockVehicle.PricingPolicy.Hourlyprice * hoursInThePark;

    mockVehicle.TicketPrice = service.CalculateTicketPrice(mockVehicle);

    Assert.Equal(expectedValue, mockVehicle.TicketPrice);
  }

  [Theory]
  [InlineData("10/10/2025 11:00:00 AM", "10")] // 1 hour
  [InlineData("10/10/2025 12:00:00 PM", "20")] // 2 hours
  [InlineData("10/10/2025 12:10:00 PM", "30")] // 3 hours
  public void CalculateTicketPrice_DepartureTimeGiven_CalculatesTheValueCorrectly(string date, string value)
  {
    var pricesTable = new Prices { Type = 0, Hourlyprice = 10.00m }; // a car
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0, PricingPolicy = pricesTable };
    mockVehicle.EntryTime = DateTime.Parse("10/10/2025 10:00:00 AM"); // enters ate 10 AM
    mockVehicle.DepartureTime = DateTime.Parse(date);
    decimal expectedValue = decimal.Parse(value);

    mockVehicle.TicketPrice = service.CalculateTicketPrice(mockVehicle);

    Assert.Equal(expectedValue, mockVehicle.TicketPrice);
  }

  [Fact]
  public void CheckingOut_DepartureTimeNotNull_ThrowsException()
  {
    var pricesTable = new Prices { Type = 0, Hourlyprice = 10.00m }; // a car
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0, PricingPolicy = pricesTable };
    mockVehicle.DepartureTime = DateTime.Parse("10/10/2025 10:00:00 AM"); // a ramdom time
    var stubDepartureTime = stubTimeProvider.GetLocalNow().DateTime;

    // the method is void, so the test acts on the assertion
    var testingException = Assert.Throws<InvalidOperationException>(() =>
        {
          service.CheckingOut(mockVehicle, stubDepartureTime);
        });
  }

  [Fact]
  public void CheckingOut_Normalcenario_AtributesTheTicketPrice()
  {
    var pricesTable = new Prices { Type = 0, Hourlyprice = 10.00m }; // a car
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0, PricingPolicy = pricesTable };
    var checkoutTime = DateTime.Parse("10/10/2025 10:00:00 AM"); // a ramdom time

    service.CheckingOut(mockVehicle, checkoutTime);

    Assert.True(mockVehicle.TicketPrice > 0);
  }

  [Fact]
  public void CheckingOut_Normalcenario_AtributesTheEffectlyHourlyPrice()
  {
    var pricesTable = new Prices { Type = 0, Hourlyprice = 10.00m }; // a car
    var mockVehicle = new Veiculo
    {
      Placa = "placa",
      Type = 0,
      EntryTime = new DateTime(2025, 10, 10, 9, 0, 0), // a random time
      PricingPolicy = pricesTable
    };
    var checkoutTime = DateTime.Parse("10/10/2025 10:00:00 AM"); // a ramdom time

    service.CheckingOut(mockVehicle, checkoutTime);

    Assert.True(mockVehicle.EffectiveHourlyPrice is not null);
  }

  [Theory]
  [InlineData("10/10/2025 11:00:00 AM")] // 1 hours
  [InlineData("10/10/2025 12:00:00 PM")] // 2 hours
  [InlineData("10/10/2025 12:10:00 PM")] // 3 hours
  public void CheckingOut_DepartureTimeNull_UsesPassedDepartureTime(string passedDepartureTime)
  {
    var pricesTable = new Prices { Type = 0, Hourlyprice = 10.00m }; // a car
    var mockVehicle = new Veiculo { Placa = "placa", Type = 0, PricingPolicy = pricesTable };
    var departureTime = DateTime.Parse(passedDepartureTime);

    service.CheckingOut(mockVehicle, departureTime);

    Assert.Equal(departureTime, mockVehicle.DepartureTime);
  }
}

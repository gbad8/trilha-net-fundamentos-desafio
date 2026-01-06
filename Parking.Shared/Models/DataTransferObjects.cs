namespace Parking.Shared.Models;

public record VeiculoToCreate(string Placa, VehicleType Type);
public record VeiculoToRead(int Id, string Placa, VehicleType Type, DateTime EntryTime, decimal TicketPrice);
public record VeiculoToUptade(int Id, DateTime DepartureTime);
public record VeiculoToDelete(int Id);

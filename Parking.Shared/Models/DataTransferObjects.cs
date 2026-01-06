namespace Parking.Shared.Models;

public record VeiculoToCreate(string Placa, VehicleType Type);
public record VeiculoToDelete(int Id);

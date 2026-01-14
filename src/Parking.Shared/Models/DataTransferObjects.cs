namespace Parking.Shared.Models;

/// <summary>
/// DTO sended to the endpoint for the creating of a new vehicle.
/// </summary>
/// <param name="Placa">User input for the new vehicle's plate.</param>
/// <param name="Type">Type of the vehicle: 0 for cars, 1 for motorcycles.</param>
public record VeiculoToCreate(string Placa, VehicleType Type);

/// <summary>
/// DTO to get a list of parked vehicles.
/// </summary>
/// <param name="Id">That's the primary key created automatically by the SQL Server.</param>
/// <param name="Placa">User input for the new vehicle plate.</param>
/// <param name="Type">Typee of the vehicle: 0 for Car, 1 for Motorcycles.</param>
/// <param name="EntryTime">The time the check-in is registered.<br/>
/// It is calculated by a service called by the check-in endpoint.</param>
/// <param name="TicketPrice">The estimated value, based on the entrytime.<br/>
/// It is calculated by the same service called by the check-in endpoint.</param>
public record VeiculoToRead(int Id, string Placa, VehicleType Type, DateTime EntryTime, decimal TicketPrice);


/// <summary>
/// DTO sended to the Patch endpoint to make the vehicle's checkout.
/// </summary>
/// <param name="Id">The id of the vehicle clicked by the user on the main page.</param>
/// <param name="DepartureTime">The departure time is atributed by a service called on the check-out page.</param>
public record VeiculoToUptade(int Id, DateTime DepartureTime);


/// <summary>
/// DTO sended to the delete endpoint to delete the corresponding vehicle from the database.
/// </summary>
/// <param name="Id">As the only necessary data to identify the original vehicle is the id, this 
/// DTO has only this property.<br/>
/// The history page has a table where the user can select how many vehicles he wants to delete from the database.</param>
public record VeiculoToDelete(int Id);

/// <summary>
/// The purpose of this DTO is to get or set the pricing policy (price per hour) for the vehicle type.
/// The vehicle type is given by the id in the uri.
/// </summary>
/// <param name="HourlyPrice"> The only parameter this DTO has is the price of the vehicle.</param> 
public record PriceToReadAndToSet(decimal HourlyPrice);

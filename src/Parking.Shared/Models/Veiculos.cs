namespace Parking.Shared.Models;

/// <summary>
/// That's the main model of the application.
/// There is a lot of information
/// someone can get with paid apis, like color of the vehicle, year of fabrications, etc.
/// However, here we have an abstraction containing only the necessary properties.
/// </summary>
public class Veiculo
{
  /// <summary>
  /// That's the primary key created by SQL Server. The primary utility of this property 
  /// is to identify the vehicle's selected by the user for marking check-out and deleting
  /// from the database.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// The Vehicle plate is the most important identifyer, but it's utility is simply visual.
  /// The user can identify and search cars based on this identification.`
  /// </summary>
  public required string Placa { get; set; }

  /// <summary>
  /// The Vehicle type identifies whether the vehicle isa car or a motorcycle.
  /// </summary>
  public required VehicleType Type { get; set; }

  /// <summary>
  /// This property marks the time the vehicle enters the parking and together with
  /// the DepartureTime, serves to calculate the vehicle`s permanency.
  /// </summary>
  public DateTime EntryTime { get; set; }


  /// <summary>
  /// Together with the EntryTime, is calculated by a service called by the endppoint.
  /// </summary>
  public DateTime? DepartureTime { get; set; }

  /// <summary>
  /// The value the vehicle`s owner must pay for the permanency. Also calculated by a sercice called from the endpoint.
  /// </summary>
  public decimal? TicketPrice { get; set; }
}

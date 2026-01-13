using System.ComponentModel.DataAnnotations;

namespace Parking.Shared.Models;

/// <summary>
/// This class represents the pricing policy that can be changed from the user's interface.
/// The Hourly price is set independently for each vehicle type and is in a one-to-many
/// relationship with the Veiculos model.
/// </summary>
public class Prices
{
  /// <summary> 
  /// The enum VehicleType is the primary key itself
  /// </summary>
  [Key]
  public VehicleType Type { get; set; }

  /// <summary>
  /// Thats the property that's going to changed from the user's interface.
  /// </summary>
  public decimal Hourlyprice { get; set; }
}

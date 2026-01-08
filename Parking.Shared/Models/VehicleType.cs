namespace Parking.Shared.Models;

using System.Text.Json.Serialization;

/// <summary>
/// This enum contains the two possible types of vehicles in the parking
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VehicleType
{
  /// <summary>
  /// The 0 is for cars
  /// </summary>
  Car = 0,

  /// <summary>
  /// The 1 is for motorcycles
  /// </summary>
  Motorcycle = 1
}

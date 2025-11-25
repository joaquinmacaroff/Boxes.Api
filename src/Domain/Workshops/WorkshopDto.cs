using System.Text.Json.Serialization;
using Boxes.WebApi.Utils;

namespace Boxes.WebApi.Domain.Workshops;

public class WorkshopDto
{
  public long Id { get; set; }

  [JsonConverter(typeof(JsonStringConverter<Address>))]
  public Address? Address { get; set; }

  public object? DefaultAddress { get; set; }
  public string? Website { get; set; }
  public string? SocialFacebook { get; set; }
  public string? SocialTwitter { get; set; }
  public string? SocialLinkedIn { get; set; }
  public TimeZone? TimeZone { get; set; }
  public object[]? Schedules { get; set; }
  public object[]? Relationships { get; set; }
  public string Type { get; set; }
  public string? Group { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
  public string? Email2 { get; set; }
  public string Phone { get; set; }
  public string? Phone2 { get; set; }
  public string? Phone3 { get; set; }
  public string FormattedAddress { get; set; }
  public bool Active { get; set; }
  public string? DefaultFormattedAddress { get; set; }
  public string? AreaCode { get; set; }
  public string? CountryCode { get; set; }
  public string? ZoneInformation { get; set; }
  public string? MakeCode { get; set; }
  public TimeSpan? TimePerShift { get; set; }
  public decimal? AmountPerShift { get; set; }
  public int? MaximumPerDay { get; set; }
  public int? MinimumAnticipationDays { get; set; }
  public object? ExternalsCrm { get; set; }
  public object? Externals { get; set; }
  public object? ResourceType { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public DateTime? RemovedAt { get; set; }
}

public class TimeZone
{
  public string Id { get; set; }
  public bool HasIanaId { get; set; }
  public string? DisplayName { get; set; }
  public string? StandardName { get; set; }
  public string? DaylightName { get; set; }
  public string? BaseUtcOffset { get; set; }
  public bool SupportsDaylightSavingTime { get; set; }
}

public class Address
{
  public string PlaceId { get; set; }
  public string Url { get; set; }
  public string? Name { get; set; }
  public string? FormattedAddress { get; set; }
  public string? AdrAddress { get; set; }
  public string? Icon { get; set; }
  public string? IconBackgroundColor { get; set; }
  public string? IconMaskBaseUri { get; set; }
  public string? Reference { get; set; }
  public List<string>? Types { get; set; }
  public string? Vicinity { get; set; }
  public List<object>? HtmlAttributions { get; set; }
  public int? UtcOffsetMinutes { get; set; }
  public Geometry? Geometry { get; set; }
  public List<AddressComponent>? AddressComponents { get; set; }
}

public class AddressComponent
{
  public string? LongName { get; set; }
  public string? ShortName { get; set; }
  public List<string>? Types { get; set; }
}

public class Geometry
{
  public Location? Location { get; set; }
  public Viewport? Viewport { get; set; }
}

public class Location
{
  public double? Lat { get; set; }
  public double? Lng { get; set; }
}

public class Viewport
{
  public double? South { get; set; }
  public double? West { get; set; }
  public double? North { get; set; }
  public double? East { get; set; }
}

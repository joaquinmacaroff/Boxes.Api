namespace Boxes.WebApi.Domain.Leads;

public class Lead
{
  public Guid Id { get; private set; }
  public int WorkshopId { get; private set; }
  public ServiceType ServiceType { get; private set; }
  public DateTime StartAt { get; private set; }
  public TimeSpan Duration { get; private set; }
  public DateTime EndAt => StartAt.Add(Duration);
  public string? Note { get; private set; }
  public Contact Contact { get; private set; }
  public Vehicle? Vehicle { get; private set; }
  /* next property represents audit columns */
  public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
  // TODO: add soft delete
  public bool Deleted { get; private set; } = false;

  private Lead() { }

  public static Lead Create(int workshopsId, ServiceType serviceType, DateTime startAt, TimeSpan duration, Contact contact, Vehicle? vehicle)
  {
    return new Lead
    {
      Id = Guid.NewGuid(),
      WorkshopId = workshopsId,
      StartAt = startAt,
      Duration = duration,
      ServiceType = serviceType,
      Contact = contact,
      Vehicle = vehicle
    };
  }
}

public enum ServiceType
{
  ScheduledMaintenance,
  ComprehensiveDiagnosis,
  ElectronicDiagnosis,
  OilChange
}

public class Contact
{
  public string Name { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
}

public record Vehicle
{
  public string Maker { get; set; }
  public string Model { get; set; }
  public int Year { get; set; }
  public string? LicensePlate { get; set; }
}

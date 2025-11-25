namespace Boxes.WebApi.Infrastructure.Clients;

public class WorkshopClientSettings
{
  public const string SectionName = "WorkshopClientSettings";
  public string EndpointUrl { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public byte CachingInHours { get; set; } = 3;
}

using Boxes.WebApi.Domain.Leads;
using Boxes.WebApi.Infrastructure.Data;
using FastEndpoints;

namespace Boxes.WebApi.Features.Leads;

public record ListLeadResponse(Guid Id, int WorkshopsId, string ServiceType,
  DateTime StartAt, DateTime EndAt, Contact Contact, Vehicle? Vehicle);


public class ListLeadEndpoint(LeadRepository repository)
    : EndpointWithoutRequest<List<ListLeadResponse>>
{
  public override void Configure()
  {
    Get("leads");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    //.../leads?workshopsId=123 => for filter by query param
    // TODO: see why swagger don't show
    int? workshopId = Query<int>("workshopsId", false);

    var leads = repository.GetAll();

    if (workshopId.HasValue && workshopId != 0)
    {
      leads = leads.Where(l => l.WorkshopId == workshopId).ToList();
    }

    var response = leads
      .OrderBy(l => l.StartAt.Date)
      .ThenBy(l => l.StartAt.Hour)
      .Select(l => new ListLeadResponse(
        l.Id,
        l.WorkshopId,
        l.ServiceType.ToString(),
        l.StartAt,
        l.EndAt,
        l.Contact,
        l.Vehicle))
      .ToList();

    await Send.OkAsync(response, ct);
  }
}

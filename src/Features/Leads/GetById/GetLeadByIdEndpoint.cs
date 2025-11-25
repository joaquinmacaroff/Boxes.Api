using Boxes.WebApi.Domain.Leads;
using Boxes.WebApi.Infrastructure.Data;
using FastEndpoints;

namespace Boxes.WebApi.Features.Leads;

public record GetLeadByIdRequest(Guid LeadId);
public record GetLeadResponse(Guid Id, int WorkshopId, DateTime StartAt, DateTime EndAt,
  string ServiceType, Contact Contact, Vehicle Vehicle);

public class GetLeadByIdEndpoint(LeadRepository repository)
  : Endpoint<GetLeadByIdRequest, GetLeadResponse>
{
  public override void Configure()
  {
    Get("leads/{LeadId}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetLeadByIdRequest req, CancellationToken ct)
  {
    var lead = repository.GetById(req.LeadId);

    GetLeadResponse response = new(
      lead.Id,
      lead.WorkshopId,
      lead.StartAt,
      lead.EndAt,
      lead.ServiceType.ToString(),
      lead.Contact,
      lead.Vehicle
    );

    await Send.OkAsync(response);
  }
}

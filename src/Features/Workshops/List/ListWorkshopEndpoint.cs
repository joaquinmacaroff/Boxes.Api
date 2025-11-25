using Boxes.WebApi.Domain.Workshops;
using Boxes.WebApi.Infrastructure.Clients;
using FastEndpoints;

namespace Boxes.WebApi.Features.Workshop;

public class ListWorkshopEndpoint(WorkshopClient client)
    : EndpointWithoutRequest<List<WorkshopDto>>
{
  public override void Configure()
  {
    Get("workshops");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var workshops = await client.GetWorkshopsAsync(ct);

    await Send.OkAsync(workshops, ct);
  }
}

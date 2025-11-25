using FastEndpoints;
using FluentValidation;
using Boxes.WebApi.Infrastructure.Data;
using Boxes.WebApi.Domain.Leads;
using Boxes.WebApi.Infrastructure.Clients;

namespace Boxes.WebApi.Features.Leads;

public record CreateLeadRequest(int WorkshopId, ServiceType ServiceType,
  DateTime StartAt, TimeSpan Duration, Contact Contact, Vehicle? Vehicle);

public record CreateLeadResponse(Guid Id, int WorkshopId, DateTime StartAt,
  DateTime EndAt, string ServiceType, Contact Contact, Vehicle? Vehicle);

public class CreateLeadEndpoint(LeadRepository repository, WorkshopClient client)
  : Endpoint<CreateLeadRequest, CreateLeadResponse>
{
  public override void Configure()
  {
    Post("leads");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateLeadRequest req, CancellationToken ct)
  {
    if (!await IsWorkshopValid(req.WorkshopId, ct))
      AddError("Workshop is not valid!");

    var lead = Lead.Create(req.WorkshopId, req.ServiceType, req.StartAt, req.Duration, req.Contact, req?.Vehicle);

    if (IsOverlap(lead))
      AddError("Lead is overlap!");

    ThrowIfAnyErrors();

    // TODO: save in persist database
    repository.Create(lead);

    CreateLeadResponse response = new(
      lead.Id,
      lead.WorkshopId,
      lead.StartAt,
      lead.EndAt,
      lead.ServiceType.ToString(),
      lead.Contact,
      lead.Vehicle
    );

    await Send.ResultAsync(TypedResults.Created("", response));
  }

  public class CreateLeadValidator : Validator<CreateLeadRequest>
  {
    public CreateLeadValidator()
    {
      RuleFor(x => x.WorkshopId).NotNull();

      RuleFor(x => x.ServiceType).NotNull();

      RuleFor(x => x.StartAt)
        .NotNull()
        .NotEmpty()
        .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
        .WithMessage("Lead date cannot be in the past.");

      RuleFor(x => x.Duration)
        .NotNull()
        .NotEmpty();

      RuleFor(x => x.Contact.Name)
        .NotNull()
        .NotEmpty();

      RuleFor(x => x.Contact.Email)
        .NotNull()
        .NotEmpty();
    }
  }

  public bool IsOverlap(Lead newLead)
  {
    var leads = repository.GetAll().Where(l => l.WorkshopId == newLead.WorkshopId);

    foreach (var lead in leads)
    {
      /* For example: 
        - Range a: Start 8, End 10
        - Range b: Start 9, End 11
        - Check: (8 < 11) is true AND (9 < 10) is true
       */
      if (newLead.StartAt < lead.EndAt && lead.StartAt < newLead.EndAt)
        return true;
    }

    return false;
  }

  public async Task<bool> IsWorkshopValid(int workshopId, CancellationToken ct)
  {
    var workshops = await client.GetWorkshopsAsync(ct);

    var validIds = workshops
      .Where(w => w.Active)
      .Select(w => w.Id)
      .ToList();

    return validIds.Contains(workshopId);
  }
}

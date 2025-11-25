using Boxes.WebApi.Domain.Leads;

namespace Boxes.WebApi.Infrastructure.Data;

// TODO: simulate seed
public class LeadRepository
{
  private readonly List<Lead> _leads = [];

  public void Create(Lead lead)
  {
    _leads.Add(lead);
  }

  public Lead? GetById(Guid id)
  {
    var lead = _leads.FirstOrDefault(l => l.Id == id);
    return lead;
  }

  public IReadOnlyList<Lead> GetAll()
  {
    return _leads.AsReadOnly();
  }

  public bool Delete(Guid id)
  {
    var lead = GetById(id);
    if (lead is null)
      return false;

    _leads.Remove(lead);
    return true;
  }
}

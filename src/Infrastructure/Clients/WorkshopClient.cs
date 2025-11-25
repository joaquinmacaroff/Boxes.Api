using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using Boxes.WebApi.Domain.Workshops;
using Microsoft.Extensions.Options;

namespace Boxes.WebApi.Infrastructure.Clients;

public class WorkshopClient
{
  private readonly HttpClient _httpClient;
  private readonly WorkshopClientSettings _workshopSettings;

  public WorkshopClient(IHttpClientFactory httpClientFactory,
    IOptions<WorkshopClientSettings> workshopSettings)
  {
    _httpClient = httpClientFactory.CreateClient("CachedClient");
    _workshopSettings = workshopSettings.Value;
  }

  public async Task<List<WorkshopDto>> GetWorkshopsAsync(CancellationToken ct)
  {
    var uriBuilder = new UriBuilder(_workshopSettings.EndpointUrl);

    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
    query["active"] = "true";
    uriBuilder.Query = query.ToString();

    string requestUri = uriBuilder.ToString();

    var authToken = Encoding.ASCII.GetBytes($"{_workshopSettings.Username}:{_workshopSettings.Password}");
    var base64AuthToken = Convert.ToBase64String(authToken);

    _httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Basic", base64AuthToken);

    var response = await _httpClient.GetAsync(requestUri, ct);

    response.EnsureSuccessStatusCode();

    var deserializeOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    var workshops = await response.Content
      .ReadFromJsonAsync<List<WorkshopDto>>(options: deserializeOptions, cancellationToken: ct);

    return workshops;
  }
}

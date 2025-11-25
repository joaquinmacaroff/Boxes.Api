using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Boxes.WebApi.Infrastructure.Clients;

public class CachingHttpMessageHandler : DelegatingHandler
{
  // TODO: move cache to redis for example
  private readonly IMemoryCache _memoryCache;
  private readonly WorkshopClientSettings _workshopSettings;

  public CachingHttpMessageHandler(IMemoryCache memoryCache,
    IOptions<WorkshopClientSettings> workshopSettings)
  {
    _memoryCache = memoryCache;
    _workshopSettings = workshopSettings.Value;
  }

  protected override async Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
  {
    if (request.Method != HttpMethod.Get)
      return await base.SendAsync(request, cancellationToken);

    var cacheKey = request.RequestUri.ToString();

    if (_memoryCache.TryGetValue(cacheKey, out byte[] cachedResponseBytes))
    {
      var cachedResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
      {
        Content = new ByteArrayContent(cachedResponseBytes)
      };

      return cachedResponse;
    }

    var response = await base.SendAsync(request, cancellationToken);

    if (response.IsSuccessStatusCode && response.Content != null)
    {
      var responseBytes = await response.Content.ReadAsByteArrayAsync();

      //TODO: move default time for caching into appsettings
      _memoryCache.Set(cacheKey, responseBytes, TimeSpan.FromHours(_workshopSettings.CachingInHours)); // Set cache expiration
    }

    return response;
  }
}

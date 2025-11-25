using System.Text.Json;
using Boxes.WebApi.Infrastructure.Clients;
using Boxes.WebApi.Infrastructure.Data;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument()
    .AddMemoryCache()
    .AddTransient<CachingHttpMessageHandler>()
    .AddSingleton<LeadRepository>()
    .AddSingleton<WorkshopClient>();

builder.Services.AddHttpClient("CachedClient")
    .AddHttpMessageHandler<CachingHttpMessageHandler>();

builder.Services
  .Configure<WorkshopClientSettings>(builder.Configuration.GetSection(WorkshopClientSettings.SectionName));

var app = builder.Build();

app.UseDefaultExceptionHandler()
    .UseFastEndpoints(c =>
    {
      c.Endpoints.RoutePrefix = "api";
      c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    })
    .UseSwaggerGen();

app.Run();

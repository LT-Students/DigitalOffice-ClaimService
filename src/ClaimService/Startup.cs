using AspNetCoreRateLimit;
using DigitalOffice.Kernel.Configurations;
using DigitalOffice.Kernel.OpenApi.SchemaFilters;
using FluentValidation;
using HealthChecks.UI.Client;
using LT.DigitalOffice.ClaimService.Business;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.Models.Dto.Configurations;
using LT.DigitalOffice.Kernel.Behaviours;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Extensions;
using LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.EFSupport.Extensions;
using LT.DigitalOffice.Kernel.EFSupport.Helpers;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ClaimService;

public class Startup : BaseApiInfo
{
  private readonly BaseServiceInfoConfig _serviceInfoConfig;
  private readonly RabbitMqConfig _rabbitMqConfig;
  private readonly SwaggerConfiguration _swaggerConfiguration;
  public const string CorsPolicyName = "LtDoCorsPolicy";
  public IConfiguration Configuration { get; }

  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;

    _serviceInfoConfig = Configuration
      .GetSection(BaseServiceInfoConfig.SectionName)
      .Get<BaseServiceInfoConfig>();

    _rabbitMqConfig = Configuration
      .GetSection(BaseRabbitMqConfig.SectionName)
      .Get<RabbitMqConfig>();

    _swaggerConfiguration = Configuration
      .GetSection(SwaggerConfiguration.SectionName)
      .Get<SwaggerConfiguration>();

    Version = "1.0";
    Description = "ClaimService is an API that intended to work with claims.";
    StartTime = DateTime.UtcNow;
    ApiName = $"LT Digital Office - {_serviceInfoConfig.Name}";
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddCors(options =>
    {
      options.AddPolicy(
        CorsPolicyName,
        builder =>
        {
          builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
    services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));
    services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));
    services.Configure<SwaggerConfiguration>(Configuration.GetSection(SwaggerConfiguration.SectionName));

    services.AddMediatR(configuration =>
    {
      configuration.RegisterServicesFromAssemblyContaining(typeof(AssemblyMarker));
    });

    services.AddHttpContextAccessor()
      .AddControllers()
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      })
      .AddNewtonsoftJson()
      .ConfigureApiBehaviorOptions(options =>
      {
        options.SuppressMapClientErrors = true;
      });

    string dbConnStr = ConnectionStringHandler.Get(Configuration);
    services.AddDbContext<ClaimServiceDbContext>(options =>
    {
      options.UseSqlServer(dbConnStr);
    });

    services.AddHealthChecks()
      .AddRabbitMqCheck()
      .AddSqlServer(dbConnStr);

    services.AddMemoryCache();

    services.Configure<IpRateLimitOptions>(options =>
      Configuration.GetSection("IpRateLimitingSettings").Bind(options));

    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    services.AddInMemoryRateLimiting();

    services.AddBusinessObjects();

    services.ConfigureMassTransit(_rabbitMqConfig);

    services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

    services.AddSwaggerGen(options =>
    {
      options.SwaggerDoc($"{Version}", new OpenApiInfo
      {
        Version = Version,
        Title = _serviceInfoConfig.Name,
        Description = Description
      });

      string controllersXmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      string modelsXmlFileName = $"{Assembly.GetAssembly(typeof(AssemblyMarker)).GetName().Name}.xml";

      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, controllersXmlFileName));
      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, modelsXmlFileName));

      options.EnableAnnotations();

      options.SchemaFilter<JsonPatchDocumentSchemaFilter>();
    });
  }

  public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
  {
    app.UpdateDatabase<ClaimServiceDbContext>();

    app.UseForwardedHeaders();

    app.UseExceptionsHandler(loggerFactory);

    app.UseApiInformation();

    app.UseRouting();

    app.UseSwagger()
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"{_swaggerConfiguration.ServicePath}/swagger/{Version}/swagger.json", $"{Version}");
      });

    app.UseMiddleware<TokenMiddleware>();

    app.UseCors(CorsPolicyName);

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers().RequireCors(CorsPolicyName);
      endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
      {
        ResultStatusCodes = new Dictionary<HealthStatus, int>
        {
          { HealthStatus.Unhealthy, 200 },
          { HealthStatus.Healthy, 200 },
          { HealthStatus.Degraded, 200 },
        },
        Predicate = check => check.Name != "masstransit-bus",
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      });
    });
  }
}

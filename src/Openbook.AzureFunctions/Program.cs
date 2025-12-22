using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Openbook.AzureFunctions.Authorization;
using Openbook.Domain;
using Openbook.GraphQL;
using Openbook.Identity;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(w => w.UseMiddleware<FunctionAuthorizationMiddleware>())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        
        services.AddApplicationDbContext(configuration["ConnectionStrings:OpenbookDatabase"] ?? throw new ArgumentNullException("ConnectionStrings:OpenbookDatabase"));
        services.AddApplicationIdentityDbContext(configuration["ConnectionStrings:IdentityDatabase"] ?? throw new ArgumentNullException("ConnectionStrings:IdentityDatabase"));
        
        services.AddSingleton<IServiceHubContext>(sp =>
        {
            var serviceManager = new ServiceManagerBuilder()
                .WithOptions(o =>
                {
                    o.ConnectionString = configuration["Azure:SignalR:ConnectionString"];
                })
                .BuildServiceManager();
            
            return serviceManager.CreateHubContextAsync("ReviewHub", CancellationToken.None)
                .GetAwaiter().GetResult();
        });

        services.AddSingleton(new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtIssuer"] ?? throw new ArgumentNullException("JwtIssuer"),
            ValidAudience = configuration["JwtAudience"] ?? throw new ArgumentNullException("JwtAudience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"] ?? throw new ArgumentNullException("JwtKey")))
        });
    })
    .AddGraphQLFunction(configure =>
    {
        configure.AddGraphQLConfiguration();
    })
    .Build();

host.Run();

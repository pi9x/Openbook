using System.Text;
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
        services.AddApplicationDbContext();
        services.AddApplicationIdentityDbContext();

        var configuration = context.Configuration;
        var issuer = configuration["JwtIssuer"] ?? throw new ArgumentNullException("JwtIssuer");
        var audience = configuration["JwtAudience"] ?? throw new ArgumentNullException("JwtAudience");
        var secret = configuration["JwtKey"] ?? throw new ArgumentNullException("JwtKey");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        services.AddSingleton(new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key
        });
    })
    .AddGraphQLFunction(configure =>
    {
        configure.AddGraphQLConfiguration();
    })
    .Build();

host.Run();

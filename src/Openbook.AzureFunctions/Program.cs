using Microsoft.Extensions.Hosting;
using Openbook.Domain;
using Openbook.GraphQL;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .AddGraphQLFunction(configure =>
    {
        configure.AddGraphQLConfiguration();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationDbContext();
    })
    .Build();

host.Run();
using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Openbook.AzureFunctions;

public class GraphQLFunction(IGraphQLRequestExecutor executor)
{
    [Function("GraphQLHttpFunction")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "graphql")]
        HttpRequestData request)
        => executor.ExecuteAsync(request);
}
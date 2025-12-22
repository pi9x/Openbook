using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Openbook.AzureFunctions.Authorization;

namespace Openbook.AzureFunctions;

public class GraphQLFunction(IGraphQLRequestExecutor executor)
{
    [FunctionAuthorize]
    [Function("GraphQLHttpFunction")]
    public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "graphql")] HttpRequestData request)
        => executor.ExecuteAsync(request);
}